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
        

        private file Dir;
        public file dir { get { return Dir; }  set { dir = Dir; OnPropertyChanged(); } }
  
        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModel()
        {

        }
        public void startTracing(string path)
        {
            Dir = searchDirectory(path);
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private file searchDirectory(string path)
        {
            var searchedDirectory = new file(path);

            string[] directoryList = Directory.GetDirectories(searchedDirectory.Name);
            foreach (var directory in directoryList)
            {
                searchedDirectory.files.Add(searchDirectory(directory));

            }
            string[] filelist = Directory.GetFiles(searchedDirectory.Name);
            foreach (var _path in filelist)
            {
                searchedDirectory.files.Add(new ViewModel.file(System.IO.Path.GetFileName(_path)));
            }
            return searchedDirectory;
        }


        public class file
        {
            public file(string name)
            {
                Name = name;
                files = new ObservableCollection<file>();
            }
            public string Name { get; set; }
            public ObservableCollection<file> files { get; set; }
        }

      

    }
}
