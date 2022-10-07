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
        private object threadLocker;
        private delegate void DirectoryHandler(object parameters);
      //  private bool isWorking = false;
        private CancellationTokenSource cancelToken = new CancellationTokenSource();
        private ParallelOptions parOpts;
        private string startedPath;
        public FilesCollection Files { get; set; }
        public ThreadsQueue queue;


        private byte percentage = 0;
        public byte Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged(nameof(Percentage)); }
        }

        private List<int> threadsId;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

      /*  public bool IsWorking { get {return isWorking; } 
            set { isWorking = value; OnPropertyChanged(nameof(IsWorking)); } }
      */


        public DirectoryTracer()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
            locker = new object();
            _pool = new Semaphore(initialCount: 10, maximumCount: 10);
            threadsId = new List<int>();
            threadLocker = new object();

            parOpts = new ParallelOptions();
            parOpts.CancellationToken = cancelToken.Token;
          //  IsWorking = false;
            queue = new ThreadsQueue(parOpts, _pool);
        }

        public void traceMainDirectory(string startedPath)
        {
            // IsWorking = true;
           // queue.IsWorking = true;
            // startedPath = "C:\\Users\\Veronika\\Downloads";
            queue.AddToQueue(startedPath, Files,handleDirectory);
            Task.Factory.StartNew(() => queue.InvokeThreadInQueue());
        }

        public void StopTracing()
        {
            //var values = Files.Values;
            cancelToken.Cancel();
           // queue.FilesStack.getSizes();
            //queue.IsWorking = false;
            //IsWorking = false;

        }

        void handleDirectory(object stateInfo)
        {
            
            lock (threadLocker)
            {
                threadsId.Add(Thread.CurrentThread.ManagedThreadId);
            }

            Array argArray = new object[2];
            argArray = (Array)stateInfo;
            string path = (string)argArray.GetValue(0);
            FilesCollection node = (FilesCollection)argArray.GetValue(1);
            
           
            var currDirectory = AddFiles(node, path);

           // AddDirectories(currDirectory, path);

            lock (threadLocker)
            {
                threadsId.Remove(Thread.CurrentThread.ManagedThreadId);
            }
            _pool.Release();
        }

        private File AddFiles(FilesCollection node, string directory)
        {
            var currDirectory = new File(directory, dispatcher,true);
            long directorySize = 0;
            
            Thread.Sleep(100);
            lock (locker)
            {
                Thread.Sleep(100);
                //Thread.Sleep(100);
                node.Add(currDirectory);
                queue.AddToStack(currDirectory);
                // filesForSizeChecking.Push(currDirectory);
               // fileStack.Add(currDirectory);
            }

            AddDirectories(currDirectory, directory);

                try
                {

                    DirectoryInfo directoryInfo = new DirectoryInfo(currDirectory.FullName);
                    FileInfo[] files = directoryInfo.GetFiles();
                    var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

                    foreach (var f in filtered)
                    {
                        if (parOpts.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                       
                    Thread.Sleep(100);
                    lock (locker)
                    {
                        currDirectory.Files.Add(new File(f.FullName, f.Length, dispatcher,false));
                    }
                    }
                }
                catch (UnauthorizedAccessException)
                {

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

    }
}
