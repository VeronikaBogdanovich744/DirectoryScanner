
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace DirectoryScannerLibrary.Models
{
    public class File:INotifyPropertyChanged
    {
        public File(string name)
        {
            Name = name;
            Files = new FilesCollection();
        }

        public File(string fullname,Dispatcher dispatcher)
        {
            FullName = fullname;
            Name = System.IO.Path.GetFileName(fullname);
            //Name = name;
            Files = new FilesCollection(dispatcher);
            id = base.GetHashCode(); 
        }

        public File(string fullname, long size, Dispatcher dispatcher)
        {
            FullName = fullname;
            Name = System.IO.Path.GetFileName(fullname);
            Files = new FilesCollection(dispatcher);
            Size = size;
            id = base.GetHashCode();
        }

        public File(string fullname, long size, Dispatcher dispatcher, bool _isDirectory)
        {
            FullName = fullname;
            Name = System.IO.Path.GetFileName(fullname);
            Files = new FilesCollection(dispatcher);
            Size = size;
            id = base.GetHashCode();
            this.IsDirectory = _isDirectory;
        }

        public File(string fullname, Dispatcher dispatcher, bool _isDirectory)
        {
            FullName = fullname;
            Name = System.IO.Path.GetFileName(fullname);
            //Name = name;
            Files = new FilesCollection(dispatcher);
            id = base.GetHashCode();
            this.IsDirectory = _isDirectory;
        }

        private bool isDirectory;
        public bool IsDirectory { get { return isDirectory; } set { isDirectory = value; OnPropertyChanged(nameof(Name)); } }
        
        private string name;
        public string Name  {get { return name; } set { name = value; OnPropertyChanged(nameof(Name)); } }

        public string FullName { get; set; }

        private long size;
        public long Size { get { return size; } set { size = value; OnPropertyChanged(nameof(Size)); } }

       
        public int id { get; private set; }

        public FilesCollection Files { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
