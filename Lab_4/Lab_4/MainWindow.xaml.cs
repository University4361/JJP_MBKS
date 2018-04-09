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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<DirectoryObject> ListOfDirectories { get; set; }
        public DirectoryObject SelectedDirectory { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ListOfDirectories = new List<DirectoryObject>();
            ListOfDirectories.Add(new DirectoryObject(0, "qwe", 0));
            ListOfDirectories.Add(new DirectoryObject(0, "qwe", 0));
            ListOfDirectories.Add(new DirectoryObject(0, "qwe", 0));
            ListOfDirectories.Add(new DirectoryObject(0, "qwe", 0));
            ListOfDirectories.Add(new DirectoryObject(0, "qwe", 0));

            MainList.ItemsSource = ListOfDirectories;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
