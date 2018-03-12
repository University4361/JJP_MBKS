using System.Collections.Generic;
using System.Linq;

namespace Mirzabaeva_lab2
{
    public class AppUser
    {
        private string _template;
        public Dictionary<char, byte> AccessDictionary;

        public string Name { get; set; }

        public AppUser(string name, string accessString, string template)
        {
            Name = name;
            _template = template;
            InitializeDictionary(accessString);
        }

        private void InitializeDictionary(string accessString)
        {
            AccessDictionary = new Dictionary<char, byte>();

            for (int i = 0; i < _template.Count(); i++)
            {
                byte accessPoint = 0;

                if (i <= accessString.Length - 1)
                    byte.TryParse(accessString[i].ToString(), out accessPoint);

                AccessDictionary.Add(_template[i], accessPoint != 1 ? (byte)0 : accessPoint);
            }
        }
    }
}
