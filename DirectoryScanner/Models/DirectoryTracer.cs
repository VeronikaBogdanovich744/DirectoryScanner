using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace DirectoryScanner.Models
{
    public class DirectoryTracer
    {

        private Dispatcher dispatcher;
        private static Semaphore _pool;
        public FilesCollection Files { get; set; }

        public DirectoryTracer()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
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

            var currDirectory = new File(Path.GetFileName(path), dispatcher);


            node.Add(currDirectory);

            //get Files
            string[] fileList = Directory.GetFiles(path);
            foreach (var filePath in fileList)
            {
                Thread.Sleep(100);

                currDirectory.Files.Add(new File(Path.GetFileName(filePath), dispatcher));

            }

            string[] directoryList = Directory.GetDirectories(path);

            foreach (var directory in directoryList)
            {
                Thread.Sleep(100);

                ThreadStart start = () => handleDirectory(new object[] { directory, currDirectory.Files });
                var t = new Thread(start);
                t.Start();
            }
            _pool.Release();

        }
    }
}
