using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        public MainWindow()
        {
            InitializeComponent();

            Closed += MainWindow_Closed; 

            string usersString = string.Empty;

            if (File.Exists(DirectoryHelper.CurrentUsersPath))
                usersString = File.ReadAllText(DirectoryHelper.CurrentUsersPath);

            Rules = new ObservableCollection<AccessRule>
            {
                new AccessRule(3, "Top secret"),
                new AccessRule(2, "Secret"),
                new AccessRule(1, "Non secret"),
                new AccessRule(0, "Default"),
            };

            List<DirectoryObject> dirs = JsonConvert.DeserializeObject<List<DirectoryObject>>(usersString);

            if (dirs != null && dirs.Any())
            {
                foreach (var item in dirs)
                    item.AccessRule = Rules.FirstOrDefault(rule => rule.AccessID == item.AccessRule.AccessID);

                ListOfDirectories = new ObservableCollection<DirectoryObject>(dirs);
            }
            else
                ListOfDirectories = new ObservableCollection<DirectoryObject>();

            MainDataGrid.ItemsSource = ListOfDirectories;
            CBItem.ItemsSource = Rules;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            var users = JsonConvert.SerializeObject(ListOfDirectories.ToList());
            File.WriteAllText(DirectoryHelper.CurrentUsersPath, users);
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

        private void CopyClick(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("Выберите исходную папку");
                return;
            }

            DirectoryObject currentDir = MainDataGrid.SelectedItem as DirectoryObject;

            if (string.IsNullOrEmpty(CopyToIdTB.Text))
            {
                System.Windows.MessageBox.Show("Введите идентификатор целевой папки");
                return;
            }

            DirectoryObject toDir = ListOfDirectories.FirstOrDefault(dir => dir.Id == int.Parse(CopyToIdTB.Text));

            if (toDir == null || toDir.Id == currentDir.Id)
            {
                System.Windows.MessageBox.Show("Введите корректное значение идентификатора папки");
                return;
            }

            if (toDir.AccessRule.AccessID < currentDir.AccessRule.AccessID)
            {
                System.Windows.MessageBox.Show("Исходная папка имеет более высокий уровень секретности. Копирование невозможно");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dirName = System.IO.Path.GetDirectoryName(openFileDialog.FileName);

                if (!dirName.Contains(currentDir.Path))
                {
                    System.Windows.MessageBox.Show("Вы выбрали файл, который находится за пределами исходной папки. Выберете другой файл");
                    return;
                }

                try
                {
                    string name = System.IO.Path.GetFileName(openFileDialog.FileName);

                    string newPath = System.IO.Path.Combine(toDir.Path, name);

                    File.WriteAllBytes(newPath, File.ReadAllBytes(openFileDialog.FileName));
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Ошибка");
                    return;
                }

                System.Windows.MessageBox.Show("Файл был успешно скопирован");
            }

        }

    }
}
