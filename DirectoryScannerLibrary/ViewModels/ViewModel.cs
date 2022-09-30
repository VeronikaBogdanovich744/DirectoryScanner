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
using System.Threading;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System.Windows.Data;
using System.Windows.Shapes;
using DirectoryScannerLibrary.Commands;
using System.Windows.Input;
using DirectoryScannerLibrary.Models;

namespace DirectoryScannerLibrary.ViewModels
{
    public class ViewModel { 

       public RelayCommand TraceDirectoryButton { get; }

        public RelayCommand StopDirectoryButton { get; }

      
        public DirectoryTracer directoryTracer;
        public ViewModel()
        {
            directoryTracer = new DirectoryTracer();

            TraceDirectoryButton = new RelayCommand(obj=> { 
                directoryTracer.traceMainDirectory(); 
            }
            );

            StopDirectoryButton = new RelayCommand(obj => {
                directoryTracer.StopTracing();
            }
            );

        }


    }
}
