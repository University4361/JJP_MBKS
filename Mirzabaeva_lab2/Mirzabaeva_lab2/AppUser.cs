using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Mirzabaeva_lab2
{
    public class AppUser
    {
        public string Template { get; set; }
        public Dictionary<char, byte> AccessDictionary { get; set; }
        public string Name { get; set; }

        public AppUser(string name, string accessString, string template)
        {
            Name = name;
            Template = template;
            InitializeDictionary(accessString);
        }

        [JsonConstructor]
        public AppUser(Dictionary<char, byte> accessDictionary, string name, string template)
        {
            AccessDictionary = accessDictionary;
            Name = name;
            Template = template;
        }

        private void InitializeDictionary(string accessString)
        {
            AccessDictionary = new Dictionary<char, byte>();

            for (int i = 0; i < Template.Count(); i++)
            {
                byte accessPoint = 0;

                if (i <= accessString.Length - 1)
                    byte.TryParse(accessString[i].ToString(), out accessPoint);

                AccessDictionary.Add(Template[i], accessPoint != 1 ? (byte)0 : accessPoint);
            }
        }
    }
}
