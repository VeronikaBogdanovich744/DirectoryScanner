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
using DirectoryScanner.Models;
using static System.Net.WebRequestMethods;

namespace DirectoryScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DirectoryTracer directoryViewModel;

        public MainWindow()
        {
            InitializeComponent();

            directoryViewModel = new DirectoryTracer();
            treeView1.ItemsSource = directoryViewModel.Files;

        }

        private void startTracing(object sender, RoutedEventArgs e)
        {
            directoryViewModel.traceMainDirectory();
        }

        private void addFile(object sender, RoutedEventArgs e)
        {
            directoryViewModel.Files.Add(new File("Testing file"));

        }
    }
}
