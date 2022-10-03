using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryScannerLibrary.Models
{
    internal class ThreadsQueue
    {
        private ConcurrentQueue<DirectoryThread> queue;
        private object threadLocker;
        internal delegate void DirectoryHandler(object parameters);
        private ParallelOptions parOpts;
        private Semaphore _pool;

        internal ThreadsQueue(ParallelOptions parallelOptions, Semaphore pool)
        {
            queue = new ConcurrentQueue<DirectoryThread>();
            threadLocker = new object();
            parOpts=parallelOptions;
            _pool=pool;
        }

        internal void AddToQueue(String directory_, FilesCollection files, DirectoryHandler handleDirectory)
        {
            DirectoryThread.Handler handler = new DirectoryThread.Handler(handleDirectory);
            lock (threadLocker)
            {
                queue.Enqueue(new DirectoryThread(handler, directory_, files));
            }

        }

        internal void InvokeThreadInQueue()
        {
            //Percentage = 0;
            while (!parOpts.CancellationToken.IsCancellationRequested)
            {
                while (!queue.IsEmpty && !parOpts.CancellationToken.IsCancellationRequested)
                {
                    _pool.WaitOne();
                    DirectoryThread thread;
                    queue.TryDequeue(out thread);
                    thread.Execute();
                }

            }
            queue.Clear();
        }
    }
}
