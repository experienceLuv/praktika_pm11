using KeeperProWpf.Data;
using KeeperProWpf.Models;
using KeeperProWpf.Session;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace KeeperProWpf
{
    public partial class GroupRequestWindow : Window
    {
        private readonly AppDbContext _context = new AppDbContext();
        private ObservableCollection<GroupVisitor> _visitors = new ObservableCollection<GroupVisitor>();

        public GroupRequestWindow()
        {
            InitializeComponent();
            VisitorsDataGrid.ItemsSource = _visitors;
            LoadDepartments();

            DateStartPicker.SelectedDate = DateTime.Today.AddDays(1);
            DateEndPicker.SelectedDate = DateTime.Today.AddDays(1);
        }

        private void LoadDepartments()
        {
            DepartmentComboBox.ItemsSource = _context.Departments.ToList();
            DepartmentComboBox.DisplayMemberPath = "DepartmentName";
            DepartmentComboBox.SelectedValuePath = "DepartmentId";
        }

        private void DepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedValue == null)
                return;

            int departmentId = (int)DepartmentComboBox.SelectedValue;

            EmployeeComboBox.ItemsSource = _context.Employees
                .Where(x => x.DepartmentId == departmentId && x.IsActive)
                .ToList();

            EmployeeComboBox.DisplayMemberPath = "FullName";
            EmployeeComboBox.SelectedValuePath = "EmployeeId";
        }

        private void AddVisitorButton_Click(object sender, RoutedEventArgs e)
        {
            _visitors.Add(new GroupVisitor
            {
                BirthDate = DateTime.Today.AddYears(-18)
            });
        }

        private void RemoveVisitorButton_Click(object sender, RoutedEventArgs e)
        {
            if (VisitorsDataGrid.SelectedItem is GroupVisitor selectedVisitor)
            {
                _visitors.Remove(selectedVisitor);
            }
        }

        private void SaveGroupRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            if (DateStartPicker.SelectedDate == null || DateEndPicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите даты.");
                return;
            }

            if (DateEndPicker.SelectedDate < DateStartPicker.SelectedDate)
            {
                MessageBox.Show("Дата окончания не может быть раньше даты начала.");
                return;
            }

            if (string.IsNullOrWhiteSpace(PurposeTextBox.Text))
            {
                MessageBox.Show("Укажите цель посещения.");
                return;
            }

            if (DepartmentComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите подразделение.");
                return;
            }

            if (EmployeeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите сотрудника.");
                return;
            }

            if (_visitors.Count < 5)
            {
                MessageBox.Show("В групповой заявке должно быть минимум 5 человек.");
                return;
            }

            foreach (var v in _visitors)
            {
                if (string.IsNullOrWhiteSpace(v.LastName) ||
                    string.IsNullOrWhiteSpace(v.FirstName) ||
                    string.IsNullOrWhiteSpace(v.Email) ||
                    string.IsNullOrWhiteSpace(v.PassportSeries) ||
                    string.IsNullOrWhiteSpace(v.PassportNumber))
                {
                    MessageBox.Show("У всех посетителей должны быть заполнены обязательные поля.");
                    return;
                }
            }

            try
            {
                int currentUserId = UserSession.CurrentUser.UserId;

                var groupType = _context.ApplicationTypes.FirstOrDefault(x => x.TypeName == "group");
                var checkingStatus = _context.ApplicationStatuses.FirstOrDefault(x => x.StatusName == "На проверке");

                if (groupType == null)
                {
                    MessageBox.Show("В базе не найден тип заявки 'group'.");
                    return;
                }

                if (checkingStatus == null)
                {
                    MessageBox.Show("В базе не найден статус 'На проверке'.");
                    return;
                }

                var application = new ApplicationEntity
                {
                    UserId = currentUserId,
                    ApplicationTypeId = groupType.ApplicationTypeId,
                    DepartmentId = (int)DepartmentComboBox.SelectedValue,
                    EmployeeId = (int)EmployeeComboBox.SelectedValue,
                    DateStart = DateTime.SpecifyKind(DateStartPicker.SelectedDate.Value.Date, DateTimeKind.Utc),
                    DateEnd = DateTime.SpecifyKind(DateEndPicker.SelectedDate.Value.Date, DateTimeKind.Utc),
                    VisitPurpose = PurposeTextBox.Text,
                    Note = NoteTextBox.Text,
                    StatusId = checkingStatus.StatusId,
                    RejectionReason = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Applications.Add(application);
                _context.SaveChanges();

                int order = 1;

                foreach (var item in _visitors)
                {
                    var visitor = new Visitor
                    {
                        LastName = item.LastName,
                        FirstName = item.FirstName,
                        MiddleName = string.IsNullOrWhiteSpace(item.MiddleName) ? null : item.MiddleName,
                        Phone = string.IsNullOrWhiteSpace(item.Phone) ? null : item.Phone,
                        Email = item.Email,
                        Organization = string.IsNullOrWhiteSpace(item.Organization) ? null : item.Organization,
                        BirthDate = DateTime.SpecifyKind(item.BirthDate.Date, DateTimeKind.Utc),
                        PassportSeries = item.PassportSeries,
                        PassportNumber = item.PassportNumber,
                        PhotoPath = null
                    };

                    _context.Visitors.Add(visitor);
                    _context.SaveChanges();

                    var link = new ApplicationVisitor
                    {
                        ApplicationId = application.ApplicationId,
                        VisitorId = visitor.VisitorId,
                        VisitorOrder = order
                    };

                    _context.ApplicationVisitors.Add(link);
                    order++;
                }

                _context.SaveChanges();

                MessageBox.Show("Групповая заявка успешно сохранена.");
                Close();
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;

                if (ex.InnerException != null)
                    errorText += "\n\nINNER: " + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    errorText += "\n\nINNER INNER: " + ex.InnerException.InnerException.Message;

                MessageBox.Show(errorText);
            }
        }
    }
}