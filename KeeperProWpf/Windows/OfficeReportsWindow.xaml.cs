using KeeperProWpf.Services;
using System.Windows;

namespace KeeperProWpf.Office.Windows
{
    public partial class OfficeReportsWindow : Window
    {
        private readonly OfficeReportService _service = new OfficeReportService();

        public OfficeReportsWindow()
        {
            InitializeComponent();
            Loaded += OfficeReportsWindow_Loaded;
        }

        private async void OfficeReportsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PeriodTypeComboBox.ItemsSource = new List<string> { "День", "Месяц", "Год" };
            PeriodTypeComboBox.SelectedIndex = 0;
            ReportDatePicker.SelectedDate = DateTime.Today;

            await LoadVisitCountReportAsync();
            await LoadCurrentPersonsReportAsync();
        }

        private async Task LoadVisitCountReportAsync()
        {
            if (PeriodTypeComboBox.SelectedItem == null || ReportDatePicker.SelectedDate == null)
                return;

            string periodType = PeriodTypeComboBox.SelectedItem.ToString()!;
            DateTime selectedDate = ReportDatePicker.SelectedDate.Value;

            VisitCountDataGrid.ItemsSource = await _service.GetVisitCountReportAsync(periodType, selectedDate);
        }

        private async Task LoadCurrentPersonsReportAsync()
        {
            CurrentPersonsDataGrid.ItemsSource = await _service.GetCurrentPersonsReportAsync();
        }

        private async void LoadVisitCountReport_Click(object sender, RoutedEventArgs e)
        {
            await LoadVisitCountReportAsync();
        }

        private async void LoadCurrentPersonsReport_Click(object sender, RoutedEventArgs e)
        {
            await LoadCurrentPersonsReportAsync();
        }
    }
}