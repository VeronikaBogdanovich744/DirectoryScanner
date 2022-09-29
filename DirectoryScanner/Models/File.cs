using DirectoryScanner.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace DirectoryScanner
{
    public class File:INotifyPropertyChanged
    {
        public File(string name)
        {
            Name = name;
            Files = new FilesCollection();
        }

        public File(string name,Dispatcher dispatcher)
        {
            Name = name;
            Files = new FilesCollection(dispatcher);
        }
        private string name;
        public string Name  {get { return name; } set { name = value; OnPropertyChanged(nameof(Name)); } }

        public FilesCollection Files { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
