using System.Windows;
using KeeperProWpf.Services;

namespace KeeperProWpf.Windows
{
    public partial class OfficeReviewWindow : Window
    {
        private readonly OfficeRequestService _service = new OfficeRequestService();
        private readonly int _applicationId;
        private bool _isBlockedByBlackList = false;

        public OfficeReviewWindow(int applicationId)
        {
            InitializeComponent();
            _applicationId = applicationId;
            Loaded += OfficeReviewWindow_Loaded;
        }

        private async void OfficeReviewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var application = await _service.GetApplicationByIdAsync(_applicationId);
            var visitors = await _service.GetVisitorsByApplicationIdAsync(_applicationId);

            if (application == null)
            {
                MessageBox.Show("Заявка не найдена.");
                Close();
                return;
            }

            TypeTextBox.Text = application.ApplicationType?.TypeName ?? "";
            DepartmentTextBox.Text = application.Department?.DepartmentName ?? "";
            EmployeeTextBox.Text = application.Employee?.FullName ?? "";
            PurposeTextBox.Text = application.VisitPurpose;

            VisitorsListBox.ItemsSource = visitors
                .Select(v => $"{v.LastName} {v.FirstName} {v.MiddleName} | Паспорт: {v.PassportSeries} {v.PassportNumber}");

            bool isBlackListed = await _service.IsBlackListedAsync(_applicationId);

            if (isBlackListed)
            {
                _isBlockedByBlackList = true;
                MessageBox.Show("Посетитель найден в черном списке. Заявка должна быть отклонена.");

                await _service.RejectApplicationAsync(_applicationId,
                    "Заявка на посещение объекта КИИ отклонена в связи с нарушением Федерального закона от 26.07.2017 № 187-ФЗ.");

                VisitDatePicker.IsEnabled = false;
                VisitTimeTextBox.IsEnabled = false;
                RejectReasonTextBox.IsEnabled = false;
            }
        }

        private async void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isBlockedByBlackList)
            {
                MessageBox.Show("Заявка уже отклонена системой.");
                return;
            }

            if (VisitDatePicker.SelectedDate == null || string.IsNullOrWhiteSpace(VisitTimeTextBox.Text))
            {
                MessageBox.Show("Укажите дату и время посещения.");
                return;
            }

            await _service.ApproveApplicationAsync(_applicationId);
            MessageBox.Show("Заявка одобрена.");
            Close();
        }

        private async void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isBlockedByBlackList)
            {
                MessageBox.Show("Заявка уже отклонена системой.");
                return;
            }

            if (string.IsNullOrWhiteSpace(RejectReasonTextBox.Text))
            {
                MessageBox.Show("Укажите причину отклонения.");
                return;
            }

            await _service.RejectApplicationAsync(_applicationId, RejectReasonTextBox.Text);
            MessageBox.Show("Заявка отклонена.");
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}