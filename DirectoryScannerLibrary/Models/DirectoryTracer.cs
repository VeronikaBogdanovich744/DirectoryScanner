using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.WebRequestMethods;

namespace DirectoryScannerLibrary.Models
{
    public class DirectoryTracer: INotifyPropertyChanged
    {

        private Dispatcher dispatcher;
        private Semaphore _pool;
        private object locker;
        private object locker2;
        //private object threadLocker;
        private delegate void DirectoryHandler(object parameters);
        //private ConcurrentQueue<DirectoryThread> _queue;
        private bool isWorking = false;
        private CancellationTokenSource cancelToken = new CancellationTokenSource();
        private ParallelOptions parOpts;
        private string startedPath;

        private ConcurrentStack<File> filesForSizeChecking;

        private byte percentage = 0;
        public byte Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged(nameof(Percentage)); }
        }

        //private List<int> threadsId;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool IsWorking { get {return isWorking; } 
            set { isWorking = value; OnPropertyChanged(nameof(IsWorking)); } }

        public FilesCollection Files { get; set; }
        ThreadsQueue queue;

        public DirectoryTracer()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
            locker = new object();
            _pool = new Semaphore(initialCount: 10, maximumCount: 10);
           // threadsId = new List<int>();
           // threadLocker = new object();
           // _queue = new ConcurrentQueue<DirectoryThread>();

            parOpts = new ParallelOptions();
            parOpts.CancellationToken = cancelToken.Token;
            IsWorking = false;
            queue = new ThreadsQueue(parOpts, _pool);

            filesForSizeChecking = new ConcurrentStack<File>();
        }

        public void traceMainDirectory()
        {
            IsWorking = true;
            startedPath = "C:\\Users\\Veronika\\Downloads";
            queue.AddToQueue(startedPath, Files,handleDirectory);
            Task.Factory.StartNew(() => queue.InvokeThreadInQueue());
        }

        public void StopTracing()
        {


            var values = Files.Values;

            cancelToken.Cancel();
            getSizes();

        }

        void handleDirectory(object stateInfo)
        {
            
           /* lock (threadLocker)
            {
                threadsId.Add(Thread.CurrentThread.ManagedThreadId);
            }*/

            Array argArray = new object[2];
            argArray = (Array)stateInfo;
            string path = (string)argArray.GetValue(0);
            FilesCollection node = (FilesCollection)argArray.GetValue(1);

            

            var currDirectory = AddFiles(node, path);

            AddDirectories(currDirectory, path);

           /* lock (threadLocker)
            {
                threadsId.Remove(Thread.CurrentThread.ManagedThreadId);
            }*/
            _pool.Release();
        }

        private File AddFiles(FilesCollection node, string directory)
        {
            var currDirectory = new File(directory, dispatcher);
            long directorySize = 0;
            lock (locker)
            {
                Thread.Sleep(200);
                node.Add(currDirectory);

               // if (!filesForSizeChecking.Contains<String>(currDirectory.FullName))
                filesForSizeChecking.Push(currDirectory);

                try
                {

                    DirectoryInfo directoryInfo = new DirectoryInfo(currDirectory.FullName);
                  //  Directory.GetDirectories(directoryInfo).Length;
                    FileInfo[] files = directoryInfo.GetFiles();
                    var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

                    foreach (var f in filtered)
                    {
                        if (parOpts.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                        
                        currDirectory.Files.Add(new File(f.FullName,f.Length, dispatcher));
                       // directorySize += f.Length;
                    }
                }
                catch (UnauthorizedAccessException)
                {

                }

            }
            return currDirectory;
        }

        private void AddDirectories(File currDirectory, string directory)
        {
            try
            {
                string[] directoryList = Directory.GetDirectories(currDirectory.FullName);

                DirectoryInfo directoryInfo = new DirectoryInfo(currDirectory.FullName);
                DirectoryInfo[] files = directoryInfo.GetDirectories();
                var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
               
                foreach (var d in filtered)
                {

                    lock (locker)
                    {
                        if (parOpts.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        queue.AddToQueue(d.FullName, currDirectory.Files, handleDirectory);

                    }
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void getSizes()
        {
            File directory;
            while (filesForSizeChecking.Count > 0)
            {
                filesForSizeChecking.TryPop(out directory);
                long size = 0;

                foreach (var file in directory.Files)
                {
                    size += file.Value.Size;
                }

                directory.Size = size;
            }
        }

    }
}
