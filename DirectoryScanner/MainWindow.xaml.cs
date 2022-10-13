using DirectoryScannerLibrary;
using Microsoft.Win32;
using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using DirectoryScannerConverters;

using static System.Net.WebRequestMethods;

namespace DirectoryScanner
{
    public partial class MainWindow : Window
    {
        public ViewModel viewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
          
            viewModel = new ViewModel();
            treeView1.ItemsSource = viewModel.directoryTracer.Files;

            this.DataContext = viewModel;
        }
        
    }
}
