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
using static DirectoryScannerLibrary.Models.ScanningMethod;

namespace DirectoryScannerLibrary.Models
{
    public class ThreadsQueue: PropertyChangedClass
    {
        private ConcurrentQueue<ScanningMethod> queue;
        private ParallelOptions parOpts;
        private Semaphore _pool;
        internal FilesStack FilesStack;

        private int isWorking;
        public int IsWorking { get { return isWorking; } set { isWorking = value;
                OnPropertyChanged(nameof(IsWorking)); } }

        internal ThreadsQueue(ParallelOptions parallelOptions, Semaphore pool)
        {
           
            queue = new ConcurrentQueue<ScanningMethod>();
            parOpts=parallelOptions;
            _pool=pool;
            FilesStack = new FilesStack();
        }

        internal void AddToQueue(WaitCallback waitCallback,String directory_, FilesCollection files)
        {
            queue.Enqueue(new ScanningMethod(waitCallback, directory_, files));
        }
        internal void AddToStack(File file)
        {
            FilesStack.Add(file);
        }

        internal void InvokeThreadInQueue()
        {
            IsWorking = 1;
            int internalNumOfThreads;
            int num1, num2;
            ThreadPool.GetAvailableThreads(out internalNumOfThreads,out num2);
            do
            {
                while (!queue.IsEmpty)
                {
                    while (!queue.IsEmpty && !parOpts.CancellationToken.IsCancellationRequested)
                    {
                        _pool.WaitOne();
                        ScanningMethod thread;
                        if (queue.TryDequeue(out thread))
                        {
                            ThreadPool.QueueUserWorkItem(thread.waitCallback, thread.sources); 

                        }
                    }
                    if (parOpts.CancellationToken.IsCancellationRequested)
                    {
                        IsWorking = 2;
                        break;
                    }
                    Thread.Sleep(500);
                }

                ThreadPool.GetAvailableThreads(out num1, out num2);
            } while (internalNumOfThreads- num1 !=0 && !parOpts.CancellationToken.IsCancellationRequested);
            IsWorking = 2;
          //  Task.Factory.StartNew(() => FilesStack.getSizes());
            FilesStack.getSizes();
            queue.Clear();
            IsWorking = 3;
        }

      
    }
}
