using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mirzabaeva_lab2
{
    /// <summary>
    /// Логика взаимодействия для TGWindow.xaml
    /// </summary>
    public partial class TGWindow : Window
    {
        AppUser _fromUser;
        AppUser _toUser;
        AppUser _removeUser;

        public TGWindow()
        {
            InitializeComponent();

            InitializeCreateObjects();

            InitializeRemoveObjects();
            InitializeRemoveStackPanels();
            SetupRemoveEvents();

            InitializeStackPanels();
            SetupEvents();
            InitializeObjects();
        }


        private void SetupEvents()
        {
            foreach (RadioButton radio in FromSP.Children.OfType<RadioButton>())
            {
                radio.Checked += Radio_Check;
            }
            
            foreach (RadioButton radio in ToSP.Children.OfType<RadioButton>())
            {
                radio.Checked += Radio_Check;
            }
        }

        private void Radio_Check(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;

            if (radio.GroupName == "From")
            {
                _fromUser = UsersWorker.AccessUsers[(string)radio.Content];
                UpdateRules(_fromUser);
            }
            else if (radio.GroupName == "To")
            {
                _toUser = UsersWorker.AccessUsers[(string)radio.Content];
            }
        }

        private void UpdateRules(AppUser user)
        {
            foreach (StackPanel stack in GrantRulesDP.Children.OfType<StackPanel>())
            {
                CheckBox check = stack.Children.OfType<CheckBox>().FirstOrDefault();

                Label label = stack.Children.OfType<Label>().FirstOrDefault();

                check.IsEnabled = user.AccessDictionary[(char)label.Content] == 1;

                check.IsChecked = false;
            }

            foreach (RadioButton radio in ToSP.Children.OfType<RadioButton>())
            {
                if (radio.Content == user.Name)
                    radio.IsEnabled = false;
                else
                    radio.IsEnabled = true;

                radio.IsChecked = false;   
            }
        }

        private void InitializeObjects()
        {
            foreach (var obj in UsersWorker.CurrentAccessObjects)
            {
                StackPanel stack = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness { Left = 2, Right = 2, Bottom = 0, Top = 0 }
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsEnabled = false
                };

                Label label = new Label()
                {
                    Content = obj
                };

                stack.Children.Add(checkBox);
                stack.Children.Add(label);

                GrantRulesDP.Children.Add(stack);
            }
        }

        private void InitializeStackPanels()
        {
            foreach (var key in UsersWorker.AccessUsers.Keys)
            {
                RadioButton fromRadioButton = new RadioButton()
                {
                    GroupName = "From",
                    Content = key
                };

                RadioButton toRadioButton = new RadioButton()
                {
                    GroupName = "To",
                    Content = key
                };

                FromSP.Children.Add(fromRadioButton);
                ToSP.Children.Add(toRadioButton);
            }
        }

        private void GrantButton_Click(object sender, RoutedEventArgs e)
        {
            if (_fromUser == null || _toUser == null)
                return;

            string objects = string.Empty;

            foreach (StackPanel stack in GrantRulesDP.Children.OfType<StackPanel>())
            {
                CheckBox check = stack.Children.OfType<CheckBox>().FirstOrDefault();

                Label label = stack.Children.OfType<Label>().FirstOrDefault();

                if (check.IsChecked ?? false)
                    objects += label.Content;
            }

            UsersWorker.GrantRules(_toUser, objects);

            Close();
        }

        private void SetupRemoveEvents()
        {
            foreach (var item in MainSP.Children)
            {
                if (item is RadioButton radio)
                {
                    radio.Checked += RemoveRadio_Check;
                }
            }
        }

        private void RemoveRadio_Check(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            _removeUser = UsersWorker.AccessUsers[(string)radio.Content];
            UpdateRemoveRules(_removeUser);
        }

        private void UpdateRemoveRules(AppUser user)
        {
            foreach (StackPanel stack in RemoveRulesDP.Children.OfType<StackPanel>())
            {
                CheckBox check = stack.Children.OfType<CheckBox>().FirstOrDefault();

                Label label = stack.Children.OfType<Label>().FirstOrDefault();

                check.IsEnabled = user.AccessDictionary[(char)label.Content] == 1;

                check.IsChecked = false;
            }
        }

        private void InitializeRemoveObjects()
        {
            foreach (var obj in UsersWorker.CurrentAccessObjects)
            {
                StackPanel stack = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness { Left = 2, Right = 2, Bottom = 0, Top = 0 }
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsEnabled = false
                };

                Label label = new Label()
                {
                    Content = obj
                };

                stack.Children.Add(checkBox);
                stack.Children.Add(label);

                RemoveRulesDP.Children.Add(stack);
            }
        }

        private void InitializeRemoveStackPanels()
        {
            foreach (var key in UsersWorker.AccessUsers.Keys)
            {
                RadioButton radioButton = new RadioButton()
                {
                    GroupName = "Test",
                    Content = key
                };

                MainSP.Children.Add(radioButton);
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_removeUser == null)
                return;

            string objects = string.Empty;

            foreach (StackPanel stack in RemoveRulesDP.Children.OfType<StackPanel>())
            {
                CheckBox check = stack.Children.OfType<CheckBox>().FirstOrDefault();

                Label label = stack.Children.OfType<Label>().FirstOrDefault();

                if (check.IsChecked ?? false)
                    objects += label.Content;
            }

            UsersWorker.RemoveRules(_removeUser, objects);

            Close();
        }

        private void InitializeCreateObjects()
        {
            foreach (var obj in UsersWorker.AllAccessObjects)
            {
                StackPanel stack = new StackPanel()
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness { Left = 2, Right = 2, Bottom = 0, Top = 0 },
                    VerticalAlignment = VerticalAlignment.Center
                };

                CheckBox checkBox = new CheckBox()
                {
                    IsChecked = UsersWorker.CurrentAccessObjects?.Contains(obj) ?? false
                };

                Label label = new Label()
                {
                    Content = obj
                };

                stack.Children.Add(checkBox);
                stack.Children.Add(label);

                CreateRulesDP.Children.Add(stack);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string objects = string.Empty;

            foreach (StackPanel stack in CreateRulesDP.Children.OfType<StackPanel>())
            {
                CheckBox check = stack.Children.OfType<CheckBox>().FirstOrDefault();

                Label label = stack.Children.OfType<Label>().FirstOrDefault();

                if (check?.IsChecked ?? false)
                    objects += label.Content;
            }

            UsersWorker.CreateSubject(NameTB.Text, objects);

            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox TB = CreateStack.Children.OfType<TextBox>().FirstOrDefault();
            CreateStack.Visibility = Visibility.Visible;
            TB.Focus();
            TB.KeyUp += TB_KeyUp;
        }

        private void TB_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox TB = sender as TextBox;

            if (e.Key != Key.Enter || string.IsNullOrEmpty(TB.Text) || UsersWorker.AllAccessObjects.Contains(TB.Text)) return;

            UsersWorker.AllAccessObjects += TB.Text;

            var objects = UsersWorker.AllAccessObjects.ToList();
            UsersWorker.AllAccessObjects = string.Empty;
            objects.Sort();
            foreach (var obj in objects)
                UsersWorker.AllAccessObjects += obj;

            CreateRulesDP.Children.Clear();
            CreateRulesDP.Children.Add(CreateStack);

            InitializeCreateObjects();

            e.Handled = true;
            CreateStack.Visibility = Visibility.Collapsed;
            TB.KeyUp -= TB_KeyUp;
        }
    }
}
