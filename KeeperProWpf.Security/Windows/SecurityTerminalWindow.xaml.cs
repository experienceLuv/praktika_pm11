using System.Windows;
using KeeperProWpf.Security.Services;
using KeeperProWpf.Security.ViewModels;

namespace KeeperProWpf.Security.Windows
{
    public partial class SecurityTerminalWindow : Window
    {
        private readonly SecurityRequestService _service = new SecurityRequestService();

        public SecurityTerminalWindow()
        {
            InitializeComponent();
            Loaded += SecurityTerminalWindow_Loaded;
        }

        private async void SecurityTerminalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TypeFilterComboBox.ItemsSource = new List<string> { "", "individual", "group" };
            await LoadDepartmentsAsync();
            await LoadApplicationsAsync();
        }

        private async Task LoadDepartmentsAsync()
        {
            var departments = await _service.GetDepartmentsAsync();
            departments.Insert(0, new Models.Department { DepartmentId = 0, DepartmentName = "Все подразделения" });

            DepartmentFilterComboBox.ItemsSource = departments;
            DepartmentFilterComboBox.DisplayMemberPath = "DepartmentName";
            DepartmentFilterComboBox.SelectedValuePath = "DepartmentId";
            DepartmentFilterComboBox.SelectedIndex = 0;
        }

        private async Task LoadApplicationsAsync()
        {
            string? type = string.IsNullOrWhiteSpace(TypeFilterComboBox.Text) ? null : TypeFilterComboBox.Text;

            int? departmentId = null;
            if (DepartmentFilterComboBox.SelectedValue is int depId && depId != 0)
                departmentId = depId;

            string? search = string.IsNullOrWhiteSpace(SearchTextBox.Text) ? null : SearchTextBox.Text;

            ApplicationsDataGrid.ItemsSource = await _service.GetApprovedApplicationsAsync(
                DateFilterPicker.SelectedDate,
                type,
                departmentId,
                search);
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadApplicationsAsync();
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            DateFilterPicker.SelectedDate = null;
            TypeFilterComboBox.SelectedIndex = 0;
            DepartmentFilterComboBox.SelectedIndex = 0;
            SearchTextBox.Text = "";
            await LoadApplicationsAsync();
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem is not SecurityRequestItemViewModel item)
            {
                MessageBox.Show("Выберите заявку.");
                return;
            }

            var window = new SecurityAccessWindow(item.ApplicationId);
            window.ShowDialog();

            _ = LoadApplicationsAsync();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}