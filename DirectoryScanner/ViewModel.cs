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

namespace DirectoryScanner
{
    public class ViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<File> Nodes { get; set; }

        public ViewModel()
        {
             Nodes = new ObservableCollection<File>();
           
        }

        public void setThreads()
        {
           /* int nWorkerThreads;
            int nCompletionThreads;
            ThreadPool.GetMaxThreads(out nWorkerThreads, out nCompletionThreads);
            MessageBox.Show("Максимальное количество потоков: " + nWorkerThreads
                + "\nПотоков ввода-вывода доступно: " + nCompletionThreads);
            for (int i = 0; i < 5; i++)
           */
                ThreadPool.SetMaxThreads(1,1);
                ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { "C:\\Users\\Veronika\\Downloads", Nodes });
           // Thread.Sleep(3000);

           // Console.ReadLine();
        }


        void handleDirectory(Object stateInfo)
        {
            Array argArray = new object[2];
            argArray = (Array)stateInfo;
            string path = (string)argArray.GetValue(0);
            ObservableCollection<File> node = (ObservableCollection<File>)argArray.GetValue(1);
           


          //  string path = stateInfo as string; //convert object to paath
           // string[] directoryList = Directory.GetDirectories(path);

            var currDirectory = new File(System.IO.Path.GetFileName(path));

           //get Files
            string[] fileList = Directory.GetFiles(path);
            foreach (var filePath in fileList)
            {
                currDirectory.files.Add(new File(System.IO.Path.GetFileName(filePath)));
            }

            Application.Current.Dispatcher.BeginInvoke(new Action(() => node.Add(currDirectory)));

            string[] directoryList = Directory.GetDirectories(path);

            foreach (var directory in directoryList)
            {
                ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { directory, currDirectory.files });
            }


            Thread.Sleep(1000);

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
        }


        private void startTracing(object sender, RoutedEventArgs e)
        {
            string[] directoryList = Directory.GetDirectories("C:\\Users\\Veronika\\Downloads");

            foreach (var directory in directoryList)
            {
                TreeViewItem item = new TreeViewItem();
                // item.Tag = drive;
                item.Header = System.IO.Path.GetFileName(directory);
                string[] fileList = Directory.GetFiles(directory);
                foreach (var path in fileList)
                {
                    item.Items.Add(System.IO.Path.GetFileName(path));
                }
            }

            string[] filelist = Directory.GetFiles("C:\\Users\\Veronika\\Downloads");
            foreach (var path in filelist)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = System.IO.Path.GetFileName(path);
            }
        }

    }
}
