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

            Closed += MainWindow_Closed;

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
            else
                Rules = new ObservableCollection<AccessRule>
                {
                    new AccessRule(0, "Default"),
                    new AccessRule(1, "Non secret"),
                    new AccessRule(2, "Secret"),
                    new AccessRule(3, "Top secret"),
                };

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
            else
                Roles = new ObservableCollection<AccessRole>
                {
                    new AccessRole(0, "Default", "0, 1", Rules),
                    new AccessRole(1, "AsdASD", "1, 2", Rules),
                    new AccessRole(2, "DSAAZZS", "2, 3", Rules),
                };

            List<MyUser> users = JsonConvert.DeserializeObject<List<MyUser>>(usersString);

            if (users != null && users.Any())
            {
                foreach (var item in users)
                    item.AllRoles = Roles;

                Users = new ObservableCollection<MyUser>(users);
            }
            else
                SetUsers();

            MainDataGrid.ItemsSource = ListOfDirectories;
            RulesDataGrid.ItemsSource = Rules;
            RolesDataGrid.ItemsSource = Roles;
            UsersDataGrid.ItemsSource = Users;

            CBItem.ItemsSource = Rules;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var folders = JsonConvert.SerializeObject(ListOfDirectories.ToList());
            File.WriteAllText(DirectoryHelper.CurrentFoldersPath, folders);

            var users = JsonConvert.SerializeObject(Users.ToList());
            File.WriteAllText(DirectoryHelper.CurrentMyUsersPath, users);

            var roles = JsonConvert.SerializeObject(Roles.ToList());
            File.WriteAllText(DirectoryHelper.CurrentRolesPath, roles);

            var rules = JsonConvert.SerializeObject(Rules.ToList());
            File.WriteAllText(DirectoryHelper.CurrentRulesPath, rules);
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            string resultPath = string.Empty;

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                    resultPath = dialog.SelectedPath;
                else
                    return;
            }

            if (ListOfDirectories.FirstOrDefault(dir => dir.Path == resultPath) != null)
                return;

            int count = Directory.CreateDirectory(resultPath).GetFiles().Length;

            ListOfDirectories.Add(new DirectoryObject(Rules.FirstOrDefault(rule => rule.AccessID == 0), resultPath, count));
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("Выберите папку, которую хотите удалить");
                return;
            }

            DirectoryObject currentDir = MainDataGrid.SelectedItem as DirectoryObject;
            currentDir.Dispose();

            ListOfDirectories.Remove(currentDir);
        }

        private void AddRuleClick(object sender, RoutedEventArgs e)
        {
            int id = Rules.Select(rule => rule.AccessID).Max() + 1;
            Rules.Add(new AccessRule(id, $"New rule {id}"));
        }

        private void DeleteRuleClick(object sender, RoutedEventArgs e)
        {
            AccessRule currentRule = RulesDataGrid.SelectedItem as AccessRule;
            Rules.Remove(currentRule);
        }

        private void AddRoleClick(object sender, RoutedEventArgs e)
        {
            int id = Roles.Select(rule => rule.RoleID).Max() + 1;
            Roles.Add(new AccessRole(id, $"New role {id}", "0,1", Rules));
        }

        private void DeleteRoleClick(object sender, RoutedEventArgs e)
        {
            AccessRole currentRole = RolesDataGrid.SelectedItem as AccessRole;
            Roles.Remove(currentRole);
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
