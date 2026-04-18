using System.Windows;
using KeeperProWpf.Security.Services;
using KeeperProWpf.Security.Session;

namespace KeeperProWpf.Security.Windows
{
    public partial class SecurityLoginWindow : Window
    {
        private readonly SecurityAuthService _authService = new SecurityAuthService();

        public SecurityLoginWindow()
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
                MessageBox.Show("Сотрудник не найден.");
                return;
            }

            SecuritySession.CurrentEmployee = employee;

            var window = new SecurityTerminalWindow();
            window.Show();

            Close();
        }
    }
}