using System.Windows;
using KeeperProWpf.Session;

namespace KeeperProWpf.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (UserSession.CurrentUser != null)
            {
                WelcomeTextBlock.Text = $"Вы вошли как: {UserSession.CurrentUser.Email}";
            }
        }

        private void OpenCreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateRequestWindow();
            window.ShowDialog();
        }

        private void OpenRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new RequestsWindow();
            window.ShowDialog();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Logout();

            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();
        }
    }
}