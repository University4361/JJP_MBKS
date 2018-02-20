using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Client_Mirzabaeva_lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _currentFilePath;
        private const string _publicFolderPath = "C:\\Users\\Public\\MirzabaevaHacker";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                    PathTB.Text = dialog.SelectedPath;
            }
        }

        private void PrivateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileNameTB.Text) || string.IsNullOrEmpty(ContentTB.Text) || string.IsNullOrEmpty(PathTB.Text))
            {
                return;
            }

            _currentFilePath = Path.Combine(PathTB.Text, FileNameTB.Text + ".txt");

            File.WriteAllText(_currentFilePath, ContentTB.Text);
        }

        private void PublicButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath) || !Directory.Exists(_publicFolderPath))
                return;

            var publicFilePath = Path.Combine(_publicFolderPath, FileNameTB.Text + ".txt");

            string data = File.ReadAllText(_currentFilePath);
            File.WriteAllText(publicFilePath, data);
        }
    }
}
