using DirectoryScanner.Models;
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

namespace DirectoryScanner
{
    public class DirectoryViewModel:INotifyPropertyChanged,INotifyCollectionChanged
    {
        /* private ConcurrentBag<File> _Files;
         public ConcurrentBag<File> Files { 
             get { 
                 return _Files; 
             }
             set { 
                 _Files = value;
                 OnCollectionChanged(nameof(Files));
             } 
         }*/
        private Dispatcher dispatcher;

        public FilesCollection Files { get; set; }

        public DirectoryViewModel()
        {
            Files = new FilesCollection();
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void traceMainDirectory()
        {
           // ThreadPool.SetMaxThreads(1, 1);
            //ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { "C:\\Users\\Veronika\\Downloads", Nodes });

            ThreadStart start = () => handleDirectory(new object[] { "C:\\Users\\Veronika\\Downloads", Files });
            var t = new Thread(start);
            t.Start();
        }
        void handleDirectory(Object stateInfo)
        {
            Array argArray = new object[2];
            argArray = (Array)stateInfo;
            string path = (string)argArray.GetValue(0);
            //ConcurrentBag<File> node = (ConcurrentBag<File>)argArray.GetValue(1);

            FilesCollection node = (FilesCollection)argArray.GetValue(1);

            var currDirectory = new File(System.IO.Path.GetFileName(path), dispatcher);

            
            node.Add(currDirectory);
                //  Application.Current.Dispatcher.BeginInvoke(new Action(() => node.Add(currDirectory)));
            
            //get Files
            string[] fileList = Directory.GetFiles(path);
            foreach (var filePath in fileList)
            {
                Thread.Sleep(100);
                currDirectory.Files.Add(new File(System.IO.Path.GetFileName(filePath), dispatcher));

            }

            string[] directoryList = Directory.GetDirectories(path);

            foreach (var directory in directoryList)
            {
                Thread.Sleep(100);
                // ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { directory, currDirectory.files });
                ThreadStart start = () => handleDirectory(new object[] { directory, currDirectory.Files });
                var t = new Thread(start);
                t.Start();
            }


            //Thread.Sleep(1000);

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCollectionChanged([CallerMemberName] String propertyName = "")
        {
            //CollectionChanged?.Invoke(this, new CollectionChangeEventArgs(NotifyCollectionChangedAction.Add));
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }
    }
}
