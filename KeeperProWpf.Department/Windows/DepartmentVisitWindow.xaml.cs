using System.Windows;
using System.Windows.Controls;
using KeeperProWpf.Department.Models;
using KeeperProWpf.Department.Services;
using KeeperProWpf.Department.Session;

namespace KeeperProWpf.Department.Windows
{
    public partial class DepartmentVisitWindow : Window
    {
        private readonly DepartmentVisitService _service = new DepartmentVisitService();
        private readonly int _applicationId;
        private List<Visitor> _visitors = new();

        public DepartmentVisitWindow(int applicationId)
        {
            InitializeComponent();
            _applicationId = applicationId;
            Loaded += DepartmentVisitWindow_Loaded;
        }

        private async void DepartmentVisitWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var app = await _service.GetApplicationByIdAsync(_applicationId);
            _visitors = await _service.GetVisitorsByApplicationIdAsync(_applicationId);

            if (app == null)
            {
                MessageBox.Show("Заявка не найдена.");
                Close();
                return;
            }

            TypeTextBox.Text = app.ApplicationType?.TypeName ?? "";
            DepartmentTextBox.Text = app.Department?.DepartmentName ?? "";
            EmployeeTextBox.Text = app.Employee?.FullName ?? "";

            VisitorsListBox.ItemsSource = _visitors.Select(v =>
                $"{v.LastName} {v.FirstName} {v.MiddleName} | Паспорт: {v.PassportSeries} {v.PassportNumber}");
        }

        private void VisitorItem_RightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is not ListBoxItem item) return;
            item.IsSelected = true;

            ContextMenu menu = new ContextMenu();
            MenuItem addBlackList = new MenuItem { Header = "Черный список..." };
            addBlackList.Click += AddBlackList_Click;
            menu.Items.Add(addBlackList);

            item.ContextMenu = menu;
        }

        private async void AddBlackList_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentSession.CurrentEmployee == null)
                return;

            int index = VisitorsListBox.SelectedIndex;
            if (index < 0 || index >= _visitors.Count)
                return;

            var selectedVisitor = _visitors[index];

            var reasonWindow = new BlackListReasonWindow
            {
                Owner = this
            };

            if (reasonWindow.ShowDialog() == true)
            {
                await _service.AddVisitorToBlackListAsync(
                    selectedVisitor.VisitorId,
                    reasonWindow.ReasonText,
                    DepartmentSession.CurrentEmployee.EmployeeId);

                MessageBox.Show("Посетитель добавлен в черный список.");
            }
        }

        private async void ConfirmArrivalButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentSession.CurrentEmployee == null)
                return;

            var result = await _service.ConfirmDepartmentArrivalAsync(
                _applicationId,
                DepartmentSession.CurrentEmployee.EmployeeId);

            MessageBox.Show(result.message);
        }

        private async void ConfirmExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (DepartmentSession.CurrentEmployee == null)
                return;

            await _service.ConfirmDepartmentExitAsync(
                _applicationId,
                DepartmentSession.CurrentEmployee.EmployeeId);

            MessageBox.Show("Время ухода подтверждено.");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}