using System.Windows;
using KeeperProWpf.Department.Services;
using KeeperProWpf.Department.Session;

namespace KeeperProWpf.Department.Windows
{
    public partial class DepartmentLoginWindow : Window
    {
        private readonly DepartmentAuthService _authService = new DepartmentAuthService();

        public DepartmentLoginWindow()
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

            DepartmentSession.CurrentEmployee = employee;

            var window = new DepartmentTerminalWindow();
            window.Show();

            Close();
        }
    }
}