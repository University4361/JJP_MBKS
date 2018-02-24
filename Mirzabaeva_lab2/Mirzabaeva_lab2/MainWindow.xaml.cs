using System.Windows;
using System.Windows.Controls;

namespace Mirzabaeva_lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string _alph = "abcdefghijklmnopqrstuvwxyz1234567890";
        private string _currentCode;
        private AppUser _currentAccessUser;

        public MainWindow()
        {
            InitializeComponent();
            AlphTextBlock.Text = _alph;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            FirstTB.Text = string.Empty;
            SecondTB.Text = string.Empty;
            ThridTB.Text = string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstRB.IsChecked ?? false)
                _currentCode = FirstTB.Text;
            if (SecondRB.IsChecked ?? false)
                _currentCode = SecondTB.Text;
            if (ThridRB.IsChecked ?? false)
                _currentCode = ThridTB.Text;

            _currentAccessUser = new AppUser(_currentCode, _alph);

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }

        private void InputTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCode) || _currentAccessUser == null)
                return;

            OutputTB.Text = string.Empty;

            foreach (var ch in InputTB.Text)
            {
                if (_currentAccessUser.AccessDictionary.ContainsKey(ch) && _currentAccessUser.AccessDictionary[ch] == 1)
                    OutputTB.Text += ch;
            }
        }
    }
}
