using KeeperProWpf.Office.Windows;
using KeeperProWpf.Services;
using KeeperProWpf.ViewModels;
using System.Windows;

namespace KeeperProWpf.Windows
{
    public partial class OfficeTerminalWindow : Window
    {
        private readonly OfficeRequestService _service = new OfficeRequestService();

        public OfficeTerminalWindow()
        {
            InitializeComponent();
            Loaded += OfficeTerminalWindow_Loaded;
            _autoReportService.Start();
        }

        private async void OfficeTerminalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TypeFilterComboBox.ItemsSource = new List<string> { "", "individual", "group" };
            await LoadDepartmentsAsync();
            await LoadStatusesAsync();
            await LoadApplicationsAsync();
        }

        private async Task LoadDepartmentsAsync()
        {
            var departments = await _service.GetDepartmentsAsync();
            departments.Insert(0, new KeeperProWpf.Models.Department { DepartmentId = 0, DepartmentName = "Все подразделения" });
            DepartmentFilterComboBox.ItemsSource = departments;
            DepartmentFilterComboBox.DisplayMemberPath = "DepartmentName";
            DepartmentFilterComboBox.SelectedValuePath = "DepartmentId";
            DepartmentFilterComboBox.SelectedIndex = 0;
        }

        private async Task LoadStatusesAsync()
        {
            var statuses = await _service.GetStatusesAsync();
            statuses.Insert(0, new KeeperProWpf.Models.ApplicationStatus { StatusId = 0, StatusName = "Все статусы" });
            StatusFilterComboBox.ItemsSource = statuses;
            StatusFilterComboBox.DisplayMemberPath = "StatusName";
            StatusFilterComboBox.SelectedValuePath = "StatusId";
            StatusFilterComboBox.SelectedIndex = 0;
        }

        private async Task LoadApplicationsAsync()
        {
            string? type = string.IsNullOrWhiteSpace(TypeFilterComboBox.Text) ? null : TypeFilterComboBox.Text;

            int? departmentId = null;
            if (DepartmentFilterComboBox.SelectedValue is int depId && depId != 0)
                departmentId = depId;

            int? statusId = null;
            if (StatusFilterComboBox.SelectedValue is int stId && stId != 0)
                statusId = stId;

            ApplicationsDataGrid.ItemsSource = await _service.GetApplicationsAsync(type, departmentId, statusId);
        }

        private async void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadApplicationsAsync();
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            TypeFilterComboBox.SelectedIndex = 0;
            DepartmentFilterComboBox.SelectedIndex = 0;
            StatusFilterComboBox.SelectedIndex = 0;
            await LoadApplicationsAsync();
        }

        private void OpenReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationsDataGrid.SelectedItem is not OfficeRequestItemViewModel item)
            {
                MessageBox.Show("Выберите заявку.");
                return;
            }

            OfficeReviewWindow window = new OfficeReviewWindow(item.ApplicationId);
            window.ShowDialog();

            _ = LoadApplicationsAsync();
        }
        private void OpenReportsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new OfficeReportsWindow();
            window.ShowDialog();
        }

        private readonly OfficeAutoReportService _autoReportService = new OfficeAutoReportService();

        private async void TestAutoReportButton_Click(object sender, RoutedEventArgs e)
        {
            await _autoReportService.GenerateVisitorsCountReportAsync(DateTime.Now);
            MessageBox.Show("Тестовый отчет сформирован.");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}