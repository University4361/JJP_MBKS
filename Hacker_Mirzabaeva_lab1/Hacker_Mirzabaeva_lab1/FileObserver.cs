using System;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Hacker_Mirzabaeva_lab1
{
    public class FileObserver
    {
        private readonly string _observePath;

        public FileObserver(string path)
        {
            _observePath = path;
            Observe();
        }

        [PermissionSet(SecurityAction.Demand)]
        public void Observe()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(_observePath)
            {
                Filter = "*.txt"
            };

            watcher.Changed += new FileSystemEventHandler(ChangeExecute);
            watcher.Created += new FileSystemEventHandler(ChangeExecute);

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Press \'c\' to close");
            while (Console.Read() != 'c') ;
        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }

        private async void ChangeExecute(object source, FileSystemEventArgs e)
        {
            Console.WriteLine(e.ChangeType + " " + e.FullPath);

            if (e.ChangeType == WatcherChangeTypes.Created)
                return;

            int i = 0;
            while (i < 20)
            {
                if (!IsFileLocked(new FileInfo(e.FullPath)))
                    break;

                i++;
                await Task.Delay(1000);
            }

            if (i == 20)
                return;

            FileWorker.CopyDataToFolder(e.FullPath, Path.Combine(Directory.GetCurrentDirectory(), "Hack"), e.Name);
        }
    }
}
