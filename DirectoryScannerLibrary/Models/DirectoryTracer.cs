using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DirectoryScannerLibrary.Models
{
    public class DirectoryTracer
    {

        private Dispatcher dispatcher;
        private Semaphore _pool;
        private object locker;
        private object threadLocker;
        private delegate void DirectoryHandler(object parameters);
        private ConcurrentQueue<DirectoryThread> _queue;
        private bool IsWorking = false;
        private CancellationTokenSource cancelToken = new CancellationTokenSource();
        private ParallelOptions parOpts;

        private List<int> threadsId;
        public FilesCollection Files { get; set; }

        public DirectoryTracer()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
            locker = new object();
            _pool = new Semaphore(initialCount: 10, maximumCount: 10);
            threadsId = new List<int>();
            threadLocker = new object();
            _queue = new ConcurrentQueue<DirectoryThread>();

            parOpts = new ParallelOptions();
            parOpts.CancellationToken = cancelToken.Token;
        }

        public void traceMainDirectory()
        {
            AddToQueue("C:\\Users\\Veronika", Files);
            Task.Factory.StartNew(() => InvokeThreadInQueue());
        }

        public void StopTracing()
        {
            cancelToken.Cancel();
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

            AddDirectories(currDirectory, path);

            lock (threadLocker)
            {
                threadsId.Remove(Thread.CurrentThread.ManagedThreadId);
            }
            _pool.Release();
        }

        private File AddFiles(FilesCollection node, string directory)
        {
            var currDirectory = new File(System.IO.Path.GetFileName(directory), dispatcher);

            lock (locker)
            {
                Thread.Sleep(100);
                node.Add(currDirectory);
                try
                {

                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    FileInfo[] files = directoryInfo.GetFiles();
                    var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

                    foreach (var f in filtered)
                    {
                        if (parOpts.CancellationToken.IsCancellationRequested)
                        {
                            break;
                        }
                        Thread.Sleep(100);
                        currDirectory.Files.Add(new File(f.Name, dispatcher));
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
                string[] directoryList = Directory.GetDirectories(directory);

                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
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
                        AddToQueue(d.FullName, currDirectory.Files);
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        private void AddToQueue(String directory_, FilesCollection files)
        {
            DirectoryThread.Handler handler = new DirectoryThread.Handler(handleDirectory);
            lock (threadLocker)
            {
                _queue.Enqueue(new DirectoryThread(handler, directory_, files));
            }

        }

        private void InvokeThreadInQueue()
        {
            while (!parOpts.CancellationToken.IsCancellationRequested)
            {
                while (!_queue.IsEmpty && !parOpts.CancellationToken.IsCancellationRequested)
                {
                    _pool.WaitOne();
                    DirectoryThread thread;
                    _queue.TryDequeue(out thread);
                    thread.Execute();
                }

            }
            _queue.Clear();
        }
    }
}
