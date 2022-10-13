using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DirectoryScannerLibrary.Models
{
    internal class ScanningMethod
    {
        internal WaitCallback waitCallback;
        internal Object[] sources;
        internal ScanningMethod(WaitCallback waitCallBack_, String path, FilesCollection node)
        {
            sources = new object[] { path, node };
            waitCallback = waitCallBack_;
        }
    }
}
