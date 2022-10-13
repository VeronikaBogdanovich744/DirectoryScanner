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
using System.Windows.Threading;
//using static System.Net.WebRequestMethods;

namespace DirectoryScannerLibrary.Models
{
    public class DirectoryTracer: PropertyChangedClass
    {
        private const int MAX_THREADS = 10;
        private Dispatcher dispatcher;
        private Semaphore _pool;
        private object locker;
        private CancellationTokenSource cancelToken;
        private ParallelOptions parOpts;
        public FilesCollection Files { get; set; }
        public ThreadsQueue queue { get; }

        private string mainDirectoryPath;


        private byte percentage = 0;
        public byte Percentage
        {
            get { return percentage; }
            set { percentage = value; OnPropertyChanged(nameof(Percentage)); }
        }

        public DirectoryTracer()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
            locker = new object();
            _pool = new Semaphore(initialCount: MAX_THREADS, maximumCount: MAX_THREADS);

            parOpts = new ParallelOptions();
            queue = new ThreadsQueue(parOpts, _pool);
        }

        public void traceMainDirectory(string startedPath)
        {
            cancelToken = new CancellationTokenSource();
            parOpts.CancellationToken = cancelToken.Token;
            Files.Clear();
            mainDirectoryPath = startedPath;
            queue.AddToQueue(new WaitCallback(handleDirectory),startedPath, Files);
            Task.Factory.StartNew(() => queue.InvokeThreadInQueue());
        }

        public void StopTracing()
        {
            if(cancelToken!=null)
            cancelToken.Cancel();
            queue.IsWorking = 2;
        }

        private void getValuesFromObject(object stateInfo, out string path,out FilesCollection node)
        {
            Array argArray = (Array)stateInfo;
            path = (string)argArray.GetValue(0);
            node = (FilesCollection)argArray.GetValue(1);
        }
        public void handleDirectory(object stateInfo)
        {
            string path;
            FilesCollection node;
            getValuesFromObject(stateInfo,out path,out node);
            AddDirectoryToCollection(node, path);
            _pool.Release();
        }
        private void  AddDirectoryToCollection(FilesCollection node, string directory)
        {
            var currDirectory = new File(directory, dispatcher,true);
            if (mainDirectoryPath == directory)
            {
                currDirectory.Percantage = 100;
            }
            
            Thread.Sleep(200);
            node.Add(currDirectory);
            queue.AddToStack(currDirectory);

            AddDirectories(currDirectory);
            AddFiles(currDirectory);
        }

        private void AddFiles(File currDirectory)
        {
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

                        Thread.Sleep(200); //under the question
                        currDirectory.Files.Add(new File(f.FullName, f.Length, dispatcher, false));
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void AddDirectories(File currDirectory)
        {
            try
            {
                string[] directoryList = Directory.GetDirectories(currDirectory.FullName);

                DirectoryInfo directoryInfo = new DirectoryInfo(currDirectory.FullName);
                DirectoryInfo[] files = directoryInfo.GetDirectories();
                var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
               
                foreach (var d in filtered)
                {

                    if (parOpts.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    queue.AddToQueue(new WaitCallback(handleDirectory),d.FullName, currDirectory.Files);
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

    }
}
