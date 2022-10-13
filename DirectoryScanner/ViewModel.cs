
using System.ComponentModel;
using DirectoryScannerLibrary.Commands;
using DirectoryScannerLibrary.Models;
using Microsoft.Win32;


namespace DirectoryScanner
{
    public class ViewModel: DirectoryScannerLibrary.PropertyChangedClass
    { 

       public RelayCommand TraceDirectoryButton { get; }

        public RelayCommand StopDirectoryButton { get; }

        public RelayCommand OpenFileDialogButton { get; }

        public DirectoryTracer directoryTracer { get; }

        public ViewModel()
        {
            directoryTracer = new DirectoryTracer();

            TraceDirectoryButton = new RelayCommand(obj=> { 
                directoryTracer.traceMainDirectory(SelectedPath); 
            }
            );

            StopDirectoryButton = new RelayCommand(obj => {
                directoryTracer.StopTracing();
            }
            );

            OpenFileDialogButton = new RelayCommand(obj => {
                OpenFile();
            }
            );
        
        }

        private string _selectedPath;

        public string SelectedPath
        {
            get { return _selectedPath; }
            set { _selectedPath = value; OnPropertyChanged(nameof(SelectedPath)); }
        }

        private void OpenFile()
        {

            var dlg = new FolderBrowserForWPF.Dialog();
            if (dlg.ShowDialog() == true)
            {
                SelectedPath = dlg.FileName;
            }
        }


    }
}
