﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DirectoryScanner.Models
{
    public class FilesCollection : ConcurrentBag<File>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        private Dispatcher dispatcher;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler != null)
                handler(this, e);
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
            if (CollectionChanged != null)
            {
                dispatcher.BeginInvoke(new Action(() => CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))));
            }
        }

    }
}
