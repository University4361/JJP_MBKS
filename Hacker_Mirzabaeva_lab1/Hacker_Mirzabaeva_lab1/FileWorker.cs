using System.IO;

namespace Hacker_Mirzabaeva_lab1
{
    public static class FileWorker
    {
        public static void CopyDataToFolder(string fromFolderPath, string toFolderPath, string fileName)
        {
            if (!Directory.Exists(toFolderPath))
                Directory.CreateDirectory(toFolderPath);

            string data = File.ReadAllText(fromFolderPath);
            File.WriteAllText(Path.Combine(toFolderPath, fileName), data);
        }
    }
}
