using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryScannerLibrary.Models
{
    internal class DirectoryThread
    {
        internal delegate void Handler(object parameters);
        private Handler handler;
        private String path;
        private FilesCollection node;
        internal int ThreadId;
        internal Thread currThread;

        internal DirectoryThread(Handler handler, String path, FilesCollection node)
        {
            this.handler = handler;
            this.path = path;
            this.node = node;
            currThread = Thread.CurrentThread;
        }

        internal Task Execute()
        {
            return Task.Factory.StartNew(() => handler(new object[] { path, node }));
        }
    }
}
