using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<DirectoryObject> ListOfDirectories { get; set; }
        
        public ObservableCollection<AccessRule> Rules { get; set; }

        public ObservableCollection<AccessRole> Roles { get; set; }

        public ObservableCollection<MyUser> Users { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            string dirsString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentFoldersPath))
                dirsString = File.ReadAllText(DirectoryHelper.CurrentFoldersPath);

            string usersString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentMyUsersPath))
                usersString = File.ReadAllText(DirectoryHelper.CurrentMyUsersPath);

            string rolesString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentRolesPath))
                rolesString = File.ReadAllText(DirectoryHelper.CurrentRolesPath);

            string rulesPath = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentRulesPath))
                rulesPath = File.ReadAllText(DirectoryHelper.CurrentRulesPath);

            List<AccessRule> rules = JsonConvert.DeserializeObject<List<AccessRule>>(rulesPath);

            if (rules != null && rules.Any())
            {
                Rules = new ObservableCollection<AccessRule>(rules);
            }

            List<DirectoryObject> dirs = JsonConvert.DeserializeObject<List<DirectoryObject>>(dirsString);

            if (dirs != null && dirs.Any())
            {
                foreach (var item in dirs)
                    item.AccessRule = Rules.FirstOrDefault(rule => rule.AccessID == item.AccessRule.AccessID);

                ListOfDirectories = new ObservableCollection<DirectoryObject>(dirs);
            }
            else
                ListOfDirectories = new ObservableCollection<DirectoryObject>();

            List<AccessRole> roles = JsonConvert.DeserializeObject<List<AccessRole>>(rolesString);

            if (roles != null && roles.Any())
            {
                foreach (var item in roles)
                    item.AllRules = Rules;

                Roles = new ObservableCollection<AccessRole>(roles);
            }

            List<MyUser> users = JsonConvert.DeserializeObject<List<MyUser>>(usersString);

            if (users != null && users.Any())
            {
                foreach (var item in users)
                    item.AllRoles = Roles;

                Users = new ObservableCollection<MyUser>(users);
            }

            MyUser myUser = Users.FirstOrDefault(user => user.UserName == Environment.UserName);

            List<AccessRule> listOfRules = new List<AccessRule>();

            foreach (var myRules in myUser.CurrentRoles.Select(role => role.SelectedRules).ToList())
                listOfRules.AddRange(myRules);

            List<DirectoryObject> objects = new List<DirectoryObject>();

            foreach (var dir in ListOfDirectories)
            {
                if (listOfRules.Select(rule => rule.AccessID).Contains(dir.AccessRule.AccessID))
                {
                    objects.Add(dir);
                }
            }

            ListOfDirectories = new ObservableCollection<DirectoryObject>(objects);

            UsernameLabel.Content = "Username: " + myUser.UserName;
            AccessLabel.Content = "Roles: " + myUser.CurrentRolesNames;

            MainDataGrid.ItemsSource = ListOfDirectories;
        }

        private void SetUsers()
        {
            Users = new ObservableCollection<MyUser>();
            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject envVar in searcher.Get())
            {
                Users.Add(new MyUser(envVar["Name"].ToString(), "0", Roles));
            }
        }

    }
}
