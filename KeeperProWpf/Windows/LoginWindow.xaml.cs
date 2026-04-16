using System.Windows;
using KeeperProWpf.Services;
using KeeperProWpf.Session;

namespace KeeperProWpf.Windows
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginSqlButton_Click(object sender, RoutedEventArgs e)
        {
            await LoginAsync("sql");
        }

        private async void LoginProcedureButton_Click(object sender, RoutedEventArgs e)
        {
            await LoginAsync("procedure");
        }

        private async void LoginOrmButton_Click(object sender, RoutedEventArgs e)
        {
            await LoginAsync("orm");
        }

        private async Task LoginAsync(string mode)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Введите email.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите пароль.");
                return;
            }

            try
            {
                var user = mode switch
                {
                    "sql" => await _authService.LoginSqlAsync(email, password),
                    "procedure" => await _authService.LoginProcedureAsync(email, password),
                    "orm" => await _authService.LoginOrmAsync(email, password),
                    _ => null
                };

                if (user == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }

                UserSession.CurrentUser = user;

                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка авторизации: " + ex.Message);
            }
        }

        private void OpenRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            Close();
        }
    }
}