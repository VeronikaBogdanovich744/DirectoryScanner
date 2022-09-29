using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace DirectoryScanner.Models
{
    public class DirectoryTracer
    {

        private Dispatcher dispatcher;
        private Semaphore _pool;
        private object locker;
        public FilesCollection Files { get; set; }

        public DirectoryTracer()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
            locker = new object();
            _pool = new Semaphore(initialCount: 5, maximumCount: 5);
        }

        public void traceMainDirectory()
        {
            ThreadStart start = () => handleDirectory(new object[] { "C:\\Users\\Veronika\\Downloads", Files });
            var t = new Thread(start);
            t.Start();
        }

        void handleDirectory(object stateInfo)
        {
            _pool.WaitOne();

            Array argArray = new object[2];
            argArray = (Array)stateInfo;
            string path = (string)argArray.GetValue(0);
            FilesCollection node = (FilesCollection)argArray.GetValue(1);
            
            var currDirectory = AddFiles(node, path);

            AddDirectories(currDirectory, path);

            _pool.Release();
        }

        private File AddFiles(FilesCollection node, string directory)
        {
            var currDirectory = new File(System.IO.Path.GetFileName(directory), dispatcher);

            lock (locker)
            {
                Thread.Sleep(100);
                node.Add(currDirectory);

                string[] fileList = Directory.GetFiles(directory);
                foreach (var filePath in fileList)
                {
                    Thread.Sleep(100);
                    currDirectory.Files.Add(new File(System.IO.Path.GetFileName(filePath), dispatcher));
                }
            }
            return currDirectory;
        }

        private void AddDirectories(File currDirectory, string directory)
        {
            string[] directoryList = Directory.GetDirectories(directory);

            foreach (var directory_ in directoryList)
            {
                lock (locker)
                {
                    ThreadStart start = () => handleDirectory(new object[] { directory_, currDirectory.Files });

                    var t = new Thread(start);
                    t.Start();
                }
            }
        }
    }
}
