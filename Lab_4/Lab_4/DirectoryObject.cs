using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_4
{
    public class DirectoryObject
    {
        public int AccessRules { get; set; }
        public string AccessRuleName { get; set; }
        public string Path { get; set; }
        public int FilesCount { get; set; }

        public DirectoryObject(int accessRules, string path, int filesCount)
        {
            AccessRules = accessRules;
            AccessRuleName = accessRules == 0 ? "Non Secret" : accessRules == 1 ? "Secret" : "Top Secret";
            Path = path;
            FilesCount = filesCount;
        }
    }
}
