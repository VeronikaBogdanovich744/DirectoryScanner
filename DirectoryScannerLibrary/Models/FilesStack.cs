using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScannerLibrary.Models
{
    internal class FilesStack
    {
        internal ConcurrentStack<File> filesForSizeChecking;
        
        internal FilesStack()
        {
            filesForSizeChecking = new ConcurrentStack<File>();

        }
        internal void getSizes()
        {
            File directory;
            while (filesForSizeChecking.Count > 0)
            {
                filesForSizeChecking.TryPop(out directory);
                long size = 0;

                foreach (var file in directory.Files)
                {
                    size += file.Value.Size;
                }

                directory.Size = size;
            }
        }

        internal void Add(File file)
        {
            filesForSizeChecking.Push(file);
        }
    }
}
