using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DirectoryScanner.Models
{
    internal class DirectoryThread
    {
        internal delegate void Handler(object parameters);
        private Handler handler;
        private String path;
        private FilesCollection node;

        internal DirectoryThread(Handler handler, String path, FilesCollection node)
        {
            this.handler = handler;
            this.path = path;
            this.node = node;

        }

        internal void Execute()
        {
            Task.Factory.StartNew(() => handler(new object[] { path, node }));
        }
    }
}
