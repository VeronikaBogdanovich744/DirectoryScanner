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

namespace DirectoryScanner
{
    public class ViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<File> Nodes { get; set; }
        public TreeView lvData { get; set; }

        public ViewModel()
        {
            
            /* Nodes = new ObservableCollection<file>();

             string[] directoryList = Directory.GetDirectories("C:\\Users\\Veronika\\Downloads");
             foreach (var directory in directoryList)
             {
                 var f = new ViewModel.file(directory);
                 string[] fileList = Directory.GetFiles(directory);
                 foreach (var file in fileList)
                 {
                     f.files.Add(new ViewModel.file(System.IO.Path.GetFileName(file)));
                 }
                 Nodes.Add(f);
                 //Nodes = nodes;
             }
             string[] filelist = Directory.GetFiles("C:\\Users\\Veronika\\Downloads");
             foreach (var path in filelist)
             {
                 Nodes.Add(new ViewModel.file(System.IO.Path.GetFileName(path)));
             }*/
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
                lvData.Items.Add(item);
                //Thread.Sleep(1000);
            }

            string[] filelist = Directory.GetFiles("C:\\Users\\Veronika\\Downloads");
            foreach (var path in filelist)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = System.IO.Path.GetFileName(path);
                lvData.Items.Add(item);
            }
        }

    }
    /*
    public class file
    {
        public file(string name)
        {
            Name = name;
            files = new ObservableCollection<file>();
        }
        public string Name { get; set; }
        public ObservableCollection<file> files { get; set; }
    }*/
}
