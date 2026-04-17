using System.Windows;
using KeeperProWpf.Services;
using KeeperProWpf.Session;

namespace KeeperProWpf.Windows
{
    public partial class OfficeLoginWindow : Window
    {
        private readonly OfficeAuthService _authService = new OfficeAuthService();

        public OfficeLoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string code = EmployeeCodeTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Введите код сотрудника.");
                return;
            }

            var employee = await _authService.LoginByCodeAsync(code);

            if (employee == null)
            {
                MessageBox.Show("Сотрудник с таким кодом не найден.");
                return;
            }

            OfficeSession.CurrentEmployee = employee;

            OfficeTerminalWindow window = new OfficeTerminalWindow();
            window.Show();

            Close();
        }
    }
}