using System.Windows;
using KeeperProWpf.Department.Services;
using KeeperProWpf.Department.Session;
using KeeperProWpf.Department.ViewModels;

namespace KeeperProWpf.Department.Windows
{
    public partial class DepartmentTerminalWindow : Window
    {
        private readonly DepartmentVisitService _service = new DepartmentVisitService();

        public DepartmentTerminalWindow()
        {
            InitializeComponent();
            Loaded += DepartmentTerminalWindow_Loaded;
        }

        private async void DepartmentTerminalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadApplicationsAsync();
        }

        private async Task LoadApplicationsAsync()
        {
            if (DepartmentSession.CurrentEmployee == null)
            {
                MessageBox.Show("Сотрудник не авторизован.");
                Close();
                return;
            }

            ApplicationsDataGrid.ItemsSource = await _service.GetApprovedDepartmentApplicationsAsync(
                DepartmentSession.CurrentEmployee.DepartmentId,
                DateFromPicker.SelectedDate,
                DateToPicker.SelectedDate);
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadApplicationsAsync();
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            DateFromPicker.SelectedDate = null;
            DateToPicker.SelectedDate = null;
            await LoadApplicationsAsync();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem is not DepartmentRequestItemViewModel item)
            {
                MessageBox.Show("Выберите заявку.");
                return;
            }

            var window = new DepartmentVisitWindow(item.ApplicationId);
            window.ShowDialog();

            _ = LoadApplicationsAsync();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}