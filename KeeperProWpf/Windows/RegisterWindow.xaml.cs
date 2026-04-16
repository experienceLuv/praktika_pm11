using System.Windows;
using KeeperProWpf.Services;

namespace KeeperProWpf.Windows
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void RegisterSqlButton_Click(object sender, RoutedEventArgs e)
        {
            await RegisterAsync("sql");
        }

        private async void RegisterProcedureButton_Click(object sender, RoutedEventArgs e)
        {
            await RegisterAsync("procedure");
        }

        private async void RegisterOrmButton_Click(object sender, RoutedEventArgs e)
        {
            await RegisterAsync("orm");
        }

        private async Task RegisterAsync(string mode)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

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

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            try
            {
                (bool Success, string Message) result = mode switch
                {
                    "sql" => await _authService.RegisterSqlAsync(email, password),
                    "procedure" => await _authService.RegisterProcedureAsync(email, password),
                    "orm" => await _authService.RegisterOrmAsync(email, password),
                    _ => (false, "Неизвестный режим регистрации.")
                };

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    var loginWindow = new LoginWindow();
                    loginWindow.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;

                if (ex.InnerException != null)
                    errorText += "\n\nINNER:\n" + ex.InnerException.Message;

                MessageBox.Show("Ошибка регистрации:\n\n" + errorText);
            }
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}