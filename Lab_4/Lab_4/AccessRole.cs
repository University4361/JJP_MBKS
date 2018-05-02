using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Lab_4
{
    public class AccessRole : INotifyPropertyChanged
    {
        private List<int> _ids;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AccessRule> AllRules;


        [JsonIgnore]
        public string AccessLevelsNames
        {
            get
            {
                string result = string.Empty;

                foreach (var rule in SelectedRules)
                    result += rule.AccessName + ", ";

                return result.TrimEnd(' ', ',');
            }
        }

        private int _roleID;
        public int RoleID
        {
            get
            {
                return _roleID;
            }
            set
            {
                _roleID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoleID"));

            }
        }

        private string _roleName;
        public string RoleName
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoleName"));
            }
        }


        private string _accessLevels;

        [JsonIgnore]
        public string AccessLevels
        {
            get
            {
                if (string.IsNullOrEmpty(_accessLevels))
                {
                    _accessLevels = string.Empty;

                    foreach (var item in SelectedRules)
                        _accessLevels += item.AccessID + ",";

                }
                return _accessLevels.TrimEnd(' ', ',');
            }
            set
            {
                if (value != null && CheckNewLevels(value.TrimEnd(' ', ',')))
                    _accessLevels = value.TrimEnd(' ', ',');

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccessLevels"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccessLevelsNames"));
            }
        }

        private ObservableCollection<AccessRule> _selectedRules;

        public ObservableCollection<AccessRule> SelectedRules
        {
            get
            {
                return _selectedRules;
            }
            set
            {
                _selectedRules = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedRules"));
            }
        }

        public AccessRole(int roleID, string roleName, string accessLevels, ObservableCollection<AccessRule> accessRules)
        {
            RoleID = roleID;
            AllRules = accessRules;
            RoleName = roleName;
            AccessLevels = accessLevels;
        }

        [JsonConstructor]
        public AccessRole(int roleID, string roleName, ObservableCollection<AccessRule> accessRules)
        {
            RoleID = roleID;
            SelectedRules = accessRules;
            RoleName = roleName;
        }

        private bool CheckNewLevels(string value)
        {
            List<int> list = new List<int>();

            foreach (var idStr in value.Split(','))
            {
                if (int.TryParse(idStr, out int id))
                {
                    if (!AllRules.Select(rule => rule.AccessID).Contains(id))
                        return false;
                    else
                        list.Add(id);
                }
                else
                    return false;
            }

            _ids = list;

            SelectedRules = new ObservableCollection<AccessRule>();

            foreach (var id in _ids)
                SelectedRules.Add(AllRules.FirstOrDefault(role => role.AccessID == id));

            return true;
        }
    }
}
