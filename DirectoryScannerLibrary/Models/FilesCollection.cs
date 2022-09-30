using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DirectoryScannerLibrary.Models
{
    public class FilesCollection : ConcurrentBag<File>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        private Dispatcher dispatcher;

        public void OnCollectionChanged()
        {
            if (CollectionChanged != null)
                dispatcher.BeginInvoke(new Action(() => CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))));
        }
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
            base.Add(file);
            OnCollectionChanged();
        }

    }
}
