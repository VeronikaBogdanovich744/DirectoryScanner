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
      //  public object _lock = new object();
        public File(string name)
        {
            Name = name;
            // files = new ObservableCollection<File>();
            // files = new ConcurrentBag<File>();
            Files = new FilesCollection();
            //  _lock = new object();
            //BindingOperations.EnableCollectionSynchronization(files, _lock);
        }

        public File(string name,Dispatcher dispatcher)
        {
            Name = name;
            // files = new ObservableCollection<File>();
            // files = new ConcurrentBag<File>();
            Files = new FilesCollection(dispatcher);
            //  _lock = new object();
            //BindingOperations.EnableCollectionSynchronization(files, _lock);
        }
        private string name;
        public string Name  {get { return name; } set {
                name = value;
                OnPropertyChanged(nameof(Name));
} }
        // public ObservableCollection<File> files { get; set; }


      /*  private ConcurrentBag<File> _files;
        public ConcurrentBag<File> files { get { return _files; } set {
                _files = value;
                OnPropertyChanged(nameof(files));
            } }
      */

        public FilesCollection Files { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
