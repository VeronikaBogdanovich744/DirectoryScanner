
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
    public class File:PropertyChangedClass//INotifyPropertyChangeded
    {
        private bool isDirectory;
        public bool IsDirectory { get { return isDirectory; } set { isDirectory = value; OnPropertyChanged(nameof(Name)); } }

        private double percantage;
        public double Percantage
        {
            get { return percantage; }
            set { percantage = value; OnPropertyChanged(nameof(Percantage)); }
        }

        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged(nameof(Name)); } }
        public string FullName { get; set; }

        private long size;
        public long Size { get { return size; } set { size = value; OnPropertyChanged(nameof(Size)); } }

        public FilesCollection Files { get; set; }
        public File(string fullname, long size, Dispatcher dispatcher, bool _isDirectory)
        {
            FullName = fullname;
            Name = System.IO.Path.GetFileName(fullname);
            Files = new FilesCollection(dispatcher);
            Size = size;
            this.IsDirectory = _isDirectory;
        }

        public File(string fullname, Dispatcher dispatcher, bool _isDirectory)
        {
            FullName = fullname;
            Name = System.IO.Path.GetFileName(fullname);
            Files = new FilesCollection(dispatcher);
            this.IsDirectory = _isDirectory;
        }
    }
}
