using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mirzabaeva_lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppUser _currentAccessUser;
        private const string _initialTemplate = "abcdefg";
        private const int _initialSubjectsCount = 4;
        private Dictionary<string, DockPanel> _panels = new Dictionary<string, DockPanel>();
        private List<RadioButton> _radios = new List<RadioButton>();

        public MainWindow()
        {
            InitializeComponent();

            InitialSetup();
            SetupMainStack();
            SetupEvents();
        }

        private void InitialSetup()
        {
            Dictionary<string, AppUser> users = new Dictionary<string, AppUser>();

            for (int i = 1; i <= _initialSubjectsCount; i++)
            {
                string name = $"User {i}";
                AppUser newUser = new AppUser(name, string.Empty, _initialTemplate);
                users.Add(name, newUser);
                UsersWorker.CurrentAccessObjects = _initialTemplate;
            }

            UsersWorker.AccessUsers = users;
        }

        private void SetupEvents()
        {
            foreach (DockPanel panel in MainStack.Children)
            {
                foreach (StackPanel stack in panel.Children.OfType<StackPanel>())
                {
                    foreach (CheckBox check in stack.Children.OfType<CheckBox>())
                    {
                        check.Checked += Check_Checked;
                        check.Unchecked += Check_Checked;
                    }
                }

                foreach (RadioButton radio in panel.Children.OfType<RadioButton>())
                {
                    radio.Checked += Check_Checked;
                }
            }
        }


        private void SetupMainStack()
        {
            MainStack.Children.Clear();
            _radios.Clear();
            _panels.Clear();

            Dictionary<string, AppUser> users = UsersWorker.AccessUsers;

            foreach (var item in users)
            {
                UsersWorker.SetupDock(item.Value, "test", out DockPanel dockPanel, out RadioButton radio);

                if (_panels.ContainsKey(item.Key))
                    _panels[item.Key] = dockPanel;
                else
                    _panels.Add(item.Key, dockPanel);

                _radios.Add(radio);

                MainStack.Children.Add(dockPanel);
            }

            SetupEvents();
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            _currentAccessUser = null;

            Dictionary<string, AppUser> users = new Dictionary<string, AppUser>();

            foreach (var item in MainStack.Children)
            {
                if (item is DockPanel dock)
                {
                    UsersWorker.SetupUser(dock, out AppUser myUser);
                    users.Add(myUser.Name, myUser);
                }
            }

            UsersWorker.AccessUsers = users;

            foreach (var panel in _panels)
            {
                if (_radios.FirstOrDefault(item => (string)item.Content == panel.Key).IsChecked ?? false)
                    UsersWorker.SetupUser(panel.Value, out _currentAccessUser);
            }

            if (_currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentAccessUser = null;

            foreach (var panel in _panels)
            {
                if (_radios.FirstOrDefault(item => (string)item.Content == panel.Key).IsChecked ?? false)
                    UsersWorker.SetupUser(panel.Value, out _currentAccessUser);
            }

            if (_currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void InputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentAccessUser = null;

            foreach (var panel in _panels)
            {
                if (_radios.FirstOrDefault(item => (string)item.Content == panel.Key).IsChecked ?? false)
                    UsersWorker.SetupUser(panel.Value, out _currentAccessUser);
            }

            if (_currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            SetupMainStack();
            OutputTB.Text = string.Empty;
            _currentAccessUser = null;
            SizeToContent = SizeToContent.WidthAndHeight;
            UpdateLayout();
        }

        private void TGButton_Click(object sender, RoutedEventArgs e)
        {
            TGWindow window = new TGWindow();
            window.Closed += Window_Closed;
            window.ShowDialog();
        }
    }
}
