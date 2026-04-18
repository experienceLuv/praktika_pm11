using System.Windows;
using KeeperProWpf.Security.Services;
using KeeperProWpf.Security.Session;

namespace KeeperProWpf.Security.Windows
{
    public partial class SecurityAccessWindow : Window
    {
        private readonly SecurityRequestService _service = new SecurityRequestService();
        private readonly int _applicationId;

        public SecurityAccessWindow(int applicationId)
        {
            InitializeComponent();
            _applicationId = applicationId;
            Loaded += SecurityAccessWindow_Loaded;
        }

        private async void SecurityAccessWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var app = await _service.GetApplicationByIdAsync(_applicationId);
            var visitors = await _service.GetVisitorsByApplicationIdAsync(_applicationId);

            if (app == null)
            {
                MessageBox.Show("Заявка не найдена.");
                Close();
                return;
            }

            TypeTextBox.Text = app.ApplicationType?.TypeName ?? "";
            DepartmentTextBox.Text = app.Department?.DepartmentName ?? "";
            EmployeeTextBox.Text = app.Employee?.FullName ?? "";

            VisitorsListBox.ItemsSource = visitors.Select(v =>
                $"{v.LastName} {v.FirstName} {v.MiddleName} | Паспорт: {v.PassportSeries} {v.PassportNumber}");
        }

        private async void AllowAccessButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecuritySession.CurrentEmployee == null)
            {
                MessageBox.Show("Сотрудник охраны не авторизован.");
                return;
            }

            await _service.AllowAccessAsync(_applicationId, SecuritySession.CurrentEmployee.EmployeeId);
            MessageBox.Show("Доступ разрешен. Время начала посещения зафиксировано.");
        }

        private async void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            await _service.SetExitTimeAsync(_applicationId);
            MessageBox.Show("Время убытия зафиксировано.");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}