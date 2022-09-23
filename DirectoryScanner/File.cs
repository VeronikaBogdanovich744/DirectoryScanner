using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner
{
    public class File
    {
        public File(string name)
        {
            Name = name;
            files = new ObservableCollection<File>();
        }
        public string Name { get; set; }
        public ObservableCollection<File> files { get; set; }
    }
}
