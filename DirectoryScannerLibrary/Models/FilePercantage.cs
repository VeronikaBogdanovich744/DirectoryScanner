using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScannerLibrary.Models
{
    internal class FilePercantage
    {
        internal static void setPercantage(File currDirectory)
        {
            foreach (var file in currDirectory.Files)
            {
                file.Value.Percantage = ((double)file.Value.Size/(double)currDirectory.Size) * 100;
            }
        }

    }
}
