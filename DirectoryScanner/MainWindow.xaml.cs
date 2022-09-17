using System;
using System.Collections.Generic;
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

namespace DirectoryScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            viewModel = new ViewModel();
        }

        private void startTracing(object sender, RoutedEventArgs e)
        {
            viewModel.startTracing("C:\\Users\\Veronika\\Downloads");

        }
    }

    public class Article
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
    }
}
