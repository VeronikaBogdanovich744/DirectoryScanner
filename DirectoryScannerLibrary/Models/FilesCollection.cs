using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DirectoryScannerLibrary.Models
{
    public class FilesCollection : ConcurrentDictionary<String,File>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public void OnCollectionChanged()
        {
            if (CollectionChanged != null)
            {
                dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))));
            }
        }

        private Dispatcher dispatcher;

        public FilesCollection() : base()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public FilesCollection(Dispatcher _dispatcher) : base()
        {
            dispatcher = _dispatcher;
        }
        public new void Add(File file)
        {
           var res =  base.TryAdd(file.FullName,file);
            OnCollectionChanged();
        }

    }
}
