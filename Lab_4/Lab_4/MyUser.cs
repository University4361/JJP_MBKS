using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Lab_4
{
    public class MyUser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<int> _ids;
        public ObservableCollection<AccessRole> AllRoles;

        private string _userName;
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        [JsonIgnore]
        public string CurrentRolesNames
        {
            get
            {
                string result = string.Empty;

                foreach (var role in CurrentRoles)
                    result += role.RoleName + ", ";

                return result.TrimEnd(' ', ',');
            }
        }


        private string _currentRolesIds;

        [JsonIgnore]
        public string CurrentRolesIds
        {
            get
            {
                if (string.IsNullOrEmpty(_currentRolesIds))
                {
                    _currentRolesIds = string.Empty;

                    foreach (var item in CurrentRoles)
                        _currentRolesIds += item.RoleID + ",";
                    
                }
                return _currentRolesIds.TrimEnd(' ', ',');
            }
            set
            {
                if (value != null && CheckNewRoles(value.TrimEnd(' ', ',')))
                    _currentRolesIds = value.TrimEnd(' ', ',');

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRolesIds"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRolesNames"));
            }
        }

        private ObservableCollection<AccessRole> _currentRoles;
        public ObservableCollection<AccessRole> CurrentRoles
        {
            get
            {
                return _currentRoles;
            }
            set
            {
                _currentRoles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRoles"));
            }
        }

        public MyUser(string name, string currentRolesIds, ObservableCollection<AccessRole> roles)
        {
            UserName = name;
            AllRoles = roles;
            CurrentRolesIds = currentRolesIds;
        }

        [JsonConstructor]
        public MyUser(string name, ObservableCollection<AccessRole> roles)
        {
            UserName = name;
            CurrentRoles = roles;
        }

        private bool CheckNewRoles(string value)
        {
            List<int> list = new List<int>();

            foreach (var idStr in value.Split(','))
            {
                if (int.TryParse(idStr, out int id))
                {
                    if (!AllRoles.Select(rule => rule.RoleID).Contains(id))
                        return false;
                    else
                        list.Add(id);
                }
                else
                    return false;
            }

            _ids = list;

            CurrentRoles = new ObservableCollection<AccessRole>();

            foreach (var id in _ids)
                CurrentRoles.Add(AllRoles.FirstOrDefault(role => role.RoleID == id));

            return true;
        }
    }
}
