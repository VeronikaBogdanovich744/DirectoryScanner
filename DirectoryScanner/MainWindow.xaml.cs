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
using DirectoryScanner.Commands;
using DirectoryScanner.Models;
using static System.Net.WebRequestMethods;

namespace DirectoryScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DirectoryTracer directoryViewModel;
        public ViewModel viewModel { get; set; }

       // public ICommand TraceDirectoryButton { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();
            treeView1.ItemsSource = viewModel.directoryTracer.Files;

            this.DataContext = viewModel;

        }
        
        private void addFile(object sender, RoutedEventArgs e)
        {
            viewModel.directoryTracer.Files.Add(new File("Testing file"));

        }
    }
}
