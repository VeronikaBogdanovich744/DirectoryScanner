using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DirectoryScanner.Commands
{
    public class FileSystem
    {

        public static void HandleDirectory(String path, ObservableCollection<File> Nodes)
        {
            ThreadPool.SetMaxThreads(1, 1);
            var dispatcher = Dispatcher.CurrentDispatcher;
          //  ThreadStart start = () => handleNode(dispatcher, path, Nodes);
           // var t = new Thread(start);
          //  t.Start();

          //  var context = SynchronizationContext.Current;
           // ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { "C:\\Users\\Veronika\\Downloads", Nodes });

        }
/*
        private static void handleNode(Dispatcher dispatcher,String path,ObservableCollection<File> node)
        {
            var currDirectory = new File(System.IO.Path.GetFileName(path));

            dispatcher.BeginInvoke(new Action(() => node.Add(currDirectory)));

            //get Files
            string[] fileList = Directory.GetFiles(path);
            foreach (var filePath in fileList)
            {
                currDirectory.files.Add(new File(System.IO.Path.GetFileName(filePath)));
            }

           // dispatcher.BeginInvoke(new Action(() => node.Add(currDirectory)));

            string[] directoryList = Directory.GetDirectories(path);

            foreach (var directory in directoryList)
            {
                ThreadStart start = () => handleNode(dispatcher, directory, currDirectory.files);
                var t = new Thread(start);
                t.Start();
                // ThreadPool.QueueUserWorkItem(handleDirectory, new object[] { directory, currDirectory.files });
            }
        }
*/
    }
}
