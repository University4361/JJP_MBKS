using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab_4
{
    public static class DirectoryHelper
    {
        static int CurrentId;

        public static string CurrentUsersPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.txt");


        static DirectoryHelper()
        {
            SetInitialId();
        }

        private static void SetInitialId()
        {
            string dirsString = string.Empty;

            if (File.Exists(CurrentUsersPath))
                dirsString = File.ReadAllText(CurrentUsersPath);

            List<DirectoryObject> dirs = JsonConvert.DeserializeObject<List<DirectoryObject>>(dirsString);

            if (dirs != null && dirs.Any())
                CurrentId = dirs.Select(dir => dir.Id).Max();
        }

        public static int GenerateID()
        {
            return ++CurrentId;
        }
    }
}
