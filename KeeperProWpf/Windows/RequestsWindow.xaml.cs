using System.Windows;
using KeeperProWpf.Services;
using KeeperProWpf.Session;

namespace KeeperProWpf.Windows
{
    public partial class RequestsWindow : Window
    {
        private readonly RequestService _requestService = new RequestService();

        public RequestsWindow()
        {
            InitializeComponent();
            Loaded += RequestsWindow_Loaded;
        }

        private async void RequestsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRequestsAsync();
        }

        private async Task LoadRequestsAsync()
        {
            try
            {
                if (UserSession.CurrentUser == null)
                {
                    MessageBox.Show("Пользователь не авторизован.");
                    Close();
                    return;
                }

                var items = await _requestService.GetUserApplicationsAsync(UserSession.CurrentUser.UserId);

                RequestsDataGrid.ItemsSource = null;
                RequestsDataGrid.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заявок: " + ex.Message);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadRequestsAsync();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}