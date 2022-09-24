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
using static System.Net.WebRequestMethods;

namespace DirectoryScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel;

        DirectoryViewModel directoryViewModel;

        public ObservableCollection<File> Nodes { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();
            directoryViewModel = new DirectoryViewModel();

            //treeView1.ItemsSource = viewModel.Nodes;

            treeView1.ItemsSource = directoryViewModel.Files;

        }

        private void startTracing(object sender, RoutedEventArgs e)
        {
            // viewModel.startTracing("C:\\Users\\Veronika\\Downloads");
            // viewModel = new ViewModel();

            // Nodes = new ObservableCollection<file>();

            /* string[] directoryList = Directory.GetDirectories("C:\\Users\\Veronika\\Downloads");
             foreach (var directory in directoryList)
             {
                 var f = new File(directory);
                 string[] fileList = Directory.GetFiles(directory);
                 foreach (var file in fileList)
                 {
                     f.files.Add(new File(System.IO.Path.GetFileName(file)));
                 }
                 viewModel.Nodes.Add(f);
                 //Nodes = nodes;
             }
             string[] filelist = Directory.GetFiles("C:\\Users\\Veronika\\Downloads");
             foreach (var path in filelist)
             {
                 viewModel.Nodes.Add(new File(System.IO.Path.GetFileName(path)));
             }*/

            
            
            //viewModel.setThreads();

            directoryViewModel.traceMainDirectory();

          //  treeView1.ItemsSource = viewModel.Nodes;

        }

        private void addFile(object sender, RoutedEventArgs e)
        {
            directoryViewModel.Files.Add(new File("Testing file"));
            
            //treeView1.ItemsSource = directoryViewModel.Files;
        }
    }
}
