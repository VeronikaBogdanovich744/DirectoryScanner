using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System.Windows.Data;
using System.Windows.Shapes;
using DirectoryScanner.Commands;
using System.Windows.Input;
using DirectoryScanner.Models;

namespace DirectoryScanner
{
    public class ViewModel { 

       public RelayCommand TraceDirectoryButton { get; }

      
        public DirectoryTracer directoryTracer;
        public ViewModel()
        {
            directoryTracer = new DirectoryTracer();
            TraceDirectoryButton = new RelayCommand(obj=> { 
                directoryTracer.traceMainDirectory(); 
            }
            );

        }
        /* public ObservableCollection<File> Nodes { get; set; }


         object _itemsLock;

         public ViewModel()
         {
              Nodes = new ObservableCollection<File>();
             _itemsLock = new object();
             BindingOperations.EnableCollectionSynchronization(Nodes, _itemsLock);

         }

         public void setThreads()
         {
               ThreadPool.SetMaxThreads(1,1);
             //ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { "C:\\Users\\Veronika\\Downloads", Nodes });

             ThreadStart start = () => handleDirectory(new object[] { "C:\\Users\\Veronika\\Downloads", Nodes });
             var t = new Thread(start);
             t.Start();
             // DirectoryScanner.Commands.FileSystem.HandleDirectory("C:\\Users\\Veronika\\Downloads", Nodes);
         }


         void handleDirectory(Object stateInfo)
         {
             Array argArray = new object[2];
             argArray = (Array)stateInfo;
             string path = (string)argArray.GetValue(0);
             ObservableCollection<File> node = (ObservableCollection<File>)argArray.GetValue(1);

             var currDirectory = new File(System.IO.Path.GetFileName(path));
             lock (_itemsLock)
             {
                 node.Add(currDirectory);
                 //  Application.Current.Dispatcher.BeginInvoke(new Action(() => node.Add(currDirectory)));
             }
             //get Files
             string[] fileList = Directory.GetFiles(path);
             foreach (var filePath in fileList)
             {
                     currDirectory.Files.Add(new File(System.IO.Path.GetFileName(filePath)));

             }

             string[] directoryList = Directory.GetDirectories(path);

             foreach (var directory in directoryList)
             {
                 // ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { directory, currDirectory.files });
                 ThreadStart start = () => handleDirectory(new object[] { directory, currDirectory.Files });
                 var t = new Thread(start);
                 t.Start();
             }


             //Thread.Sleep(1000);

         }


         static void JobForAThread(object state)
         {         
             MessageBox.Show("Поток "+ Thread.CurrentThread.ManagedThreadId);          
             Thread.Sleep(50);
         }


         public event PropertyChangedEventHandler? PropertyChanged;

         protected void OnPropertyChanged(string propertyName)
         {
            // VerifyPropertyName(propertyName);
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
         }*/


    }
}
