using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DirectoryScannerLibrary.Models
{
    public class ThreadsQueue
    {
        private ConcurrentQueue<DirectoryThread> queue;
        private object threadLocker;
        internal delegate void DirectoryHandler(object parameters);
        private ParallelOptions parOpts;
        private Semaphore _pool;
        internal FilesStack FilesStack;
        //private List<Thread> threads;

        internal ThreadsQueue(ParallelOptions parallelOptions, Semaphore pool)
        {
            queue = new ConcurrentQueue<DirectoryThread>();
            threadLocker = new object();
            parOpts=parallelOptions;
            _pool=pool;
            FilesStack = new FilesStack();
           // threads = new List<Thread>();
        }

        internal void AddToQueue(String directory_, FilesCollection files, DirectoryHandler handleDirectory)
        {
            DirectoryThread.Handler handler = new DirectoryThread.Handler(handleDirectory);
            lock (threadLocker)
            {
                queue.Enqueue(new DirectoryThread(handler, directory_, files));
            }

        }
        internal void AddToStack(File file)
        {
            FilesStack.Add(file);
        }

        internal void InvokeThreadInQueue()
        {
            bool isFirstLoop = true;
            List<Task> tasks = new List<Task>();
            do
            {
                while (!queue.IsEmpty)
                {

                    if (parOpts.CancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    _pool.WaitOne();
                    DirectoryThread thread;
                    if (queue.TryDequeue(out thread))
                    {
                        tasks.Add(thread.Execute());
                       // threads.Add(thread.currThread);
                    }

                    if (isFirstLoop)
                    {
                        isFirstLoop = false;
                        Task.WaitAll(tasks.ToArray(), parOpts.CancellationToken);
                    }
                }
               if (Task.WaitAll(tasks.ToArray(),1000) && queue.IsEmpty)
                    break;
            } while (!parOpts.CancellationToken.IsCancellationRequested);

            Task.Factory.StartNew(() => FilesStack.getSizes());
            queue.Clear();
        }

      
    }
}
