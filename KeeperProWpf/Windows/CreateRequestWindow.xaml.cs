using KeeperProWpf.Models;
using KeeperProWpf.Services;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace KeeperProWpf.Windows
{
    public partial class CreateRequestWindow : Window
    {
        private readonly RequestService _requestService = new RequestService();

        private string? _selectedPhotoPath;
        private string? _selectedPassportPdfPath;
        private bool _isUpdatingPhone;

        public CreateRequestWindow()
        {
            InitializeComponent();
            Loaded += CreateRequestWindow_Loaded;
        }

        private async void CreateRequestWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDepartmentsAsync();

            DateStartPicker.SelectedDate = DateTime.Today.AddDays(1);
            DateEndPicker.SelectedDate = DateTime.Today.AddDays(1);
            BirthDatePicker.SelectedDate = DateTime.Today.AddYears(-16);

            PhoneTextBox.Text = "+7 (";
        }

        private async Task LoadDepartmentsAsync()
        {
            try
            {
                var departments = await _requestService.GetDepartmentsAsync();
                DepartmentComboBox.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки подразделений: " + ex.Message);
            }
        }

        private async void DepartmentComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DepartmentComboBox.SelectedItem is not Department department)
                return;

            try
            {
                var employees = await _requestService.GetEmployeesByDepartmentAsync(department.DepartmentId);
                EmployeeComboBox.ItemsSource = employees;
                EmployeeComboBox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки сотрудников: " + ex.Message);
            }
        }

        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "JPG files (*.jpg;*.jpeg)|*.jpg;*.jpeg"
            };

            if (dialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(dialog.FileName);

                if (fileInfo.Length > 4 * 1024 * 1024)
                {
                    MessageBox.Show("Фото должно быть не больше 4 МБ.");
                    return;
                }

                if (!IsPhotoAspectRatioThreeByFour(dialog.FileName))
                {
                    MessageBox.Show("Фото должно иметь соотношение сторон 3x4.");
                    return;
                }

                _selectedPhotoPath = dialog.FileName;
                PhotoPathTextBlock.Text = dialog.FileName;
            }
        }

        private void SelectPassportPdfButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                _selectedPassportPdfPath = dialog.FileName;
                PassportPdfPathTextBlock.Text = dialog.FileName;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                var department = (Department)DepartmentComboBox.SelectedItem;
                var employee = (Employee)EmployeeComboBox.SelectedItem;

                string? phone = null;
                string digits = ExtractPhoneDigits(PhoneTextBox.Text);

                if (!string.IsNullOrWhiteSpace(digits))
                    phone = PhoneTextBox.Text.Trim();

                await _requestService.SaveIndividualRequestAsync(
                    dateStart: DateStartPicker.SelectedDate!.Value.Date,
                    dateEnd: DateEndPicker.SelectedDate!.Value.Date,
                    visitPurpose: VisitPurposeTextBox.Text.Trim(),
                    departmentId: department.DepartmentId,
                    employeeId: employee.EmployeeId,
                    lastName: LastNameTextBox.Text.Trim(),
                    firstName: FirstNameTextBox.Text.Trim(),
                    middleName: string.IsNullOrWhiteSpace(MiddleNameTextBox.Text) ? null : MiddleNameTextBox.Text.Trim(),
                    phone: phone,
                    email: VisitorEmailTextBox.Text.Trim(),
                    organization: string.IsNullOrWhiteSpace(OrganizationTextBox.Text) ? null : OrganizationTextBox.Text.Trim(),
                    note: NoteTextBox.Text.Trim(),
                    birthDate: BirthDatePicker.SelectedDate!.Value.Date,
                    passportSeries: PassportSeriesTextBox.Text.Trim(),
                    passportNumber: PassportNumberTextBox.Text.Trim(),
                    photoSourcePath: _selectedPhotoPath,
                    passportPdfSourcePath: _selectedPassportPdfPath!
                );

                MessageBox.Show("Заявка успешно создана.");
                ClearForm();
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;

                if (ex.InnerException != null)
                    errorText += "\n\nINNER:\n" + ex.InnerException.Message;

                MessageBox.Show("Ошибка сохранения заявки:\n\n" + errorText);
            }
        }

        private bool ValidateForm()
        {
            if (DateStartPicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату начала.");
                return false;
            }

            if (DateEndPicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату окончания.");
                return false;
            }

            DateTime start = DateStartPicker.SelectedDate.Value.Date;
            DateTime end = DateEndPicker.SelectedDate.Value.Date;

            if (start < DateTime.Today.AddDays(1))
            {
                MessageBox.Show("Дата начала должна быть не раньше следующего дня.");
                return false;
            }

            if (start > DateTime.Today.AddDays(15))
            {
                MessageBox.Show("Дата начала должна быть не позже чем через 15 дней.");
                return false;
            }

            if (end < start)
            {
                MessageBox.Show("Дата окончания не может быть раньше даты начала.");
                return false;
            }

            if (end > start.AddDays(15))
            {
                MessageBox.Show("Дата окончания должна быть не позже чем через 15 дней от даты начала.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(VisitPurposeTextBox.Text))
            {
                MessageBox.Show("Введите цель посещения.");
                return false;
            }

            if (DepartmentComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите подразделение.");
                return false;
            }

            if (EmployeeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите сотрудника.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Введите фамилию.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("Введите имя.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(VisitorEmailTextBox.Text))
            {
                MessageBox.Show("Введите email посетителя.");
                return false;
            }

            if (!Regex.IsMatch(VisitorEmailTextBox.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Некорректный email.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(NoteTextBox.Text))
            {
                MessageBox.Show("Введите примечание.");
                return false;
            }

            if (BirthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения.");
                return false;
            }

            DateTime birthDate = BirthDatePicker.SelectedDate.Value.Date;
            int age = DateTime.Today.Year - birthDate.Year;
            if (birthDate > DateTime.Today.AddYears(-age))
                age--;

            if (age < 16)
            {
                MessageBox.Show("Посетитель должен быть не моложе 16 лет.");
                return false;
            }

            string phoneDigits = ExtractPhoneDigits(PhoneTextBox.Text);
            if (!string.IsNullOrWhiteSpace(phoneDigits) && phoneDigits.Length != 11)
            {
                MessageBox.Show("Телефон должен быть в формате +7 (###) ###-##-##.");
                return false;
            }

            string passportSeries = PassportSeriesTextBox.Text.Trim();
            string passportNumber = PassportNumberTextBox.Text.Trim();

            if (passportSeries.Length != 4 || !passportSeries.All(char.IsDigit))
            {
                MessageBox.Show("Серия паспорта должна содержать 4 цифры.");
                return false;
            }

            if (passportNumber.Length != 6 || !passportNumber.All(char.IsDigit))
            {
                MessageBox.Show("Номер паспорта должен содержать 6 цифр.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_selectedPassportPdfPath))
            {
                MessageBox.Show("Выберите PDF со сканом паспорта.");
                return false;
            }

            if (Path.GetExtension(_selectedPassportPdfPath).ToLower() != ".pdf")
            {
                MessageBox.Show("Скан паспорта должен быть в формате PDF.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(_selectedPhotoPath))
            {
                string ext = Path.GetExtension(_selectedPhotoPath).ToLower();
                if (ext != ".jpg" && ext != ".jpeg")
                {
                    MessageBox.Show("Фото должно быть в формате JPG.");
                    return false;
                }

                FileInfo fileInfo = new FileInfo(_selectedPhotoPath);
                if (fileInfo.Length > 4 * 1024 * 1024)
                {
                    MessageBox.Show("Фото должно быть не больше 4 МБ.");
                    return false;
                }

                if (!IsPhotoAspectRatioThreeByFour(_selectedPhotoPath))
                {
                    MessageBox.Show("Фото должно иметь соотношение сторон 3x4.");
                    return false;
                }
            }

            return true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            DateStartPicker.SelectedDate = DateTime.Today.AddDays(1);
            DateEndPicker.SelectedDate = DateTime.Today.AddDays(1);

            VisitPurposeTextBox.Clear();
            DepartmentComboBox.SelectedIndex = -1;
            EmployeeComboBox.ItemsSource = null;

            LastNameTextBox.Clear();
            FirstNameTextBox.Clear();
            MiddleNameTextBox.Clear();
            PhoneTextBox.Text = "+7 (";
            VisitorEmailTextBox.Clear();
            OrganizationTextBox.Clear();
            NoteTextBox.Clear();
            BirthDatePicker.SelectedDate = DateTime.Today.AddYears(-16);
            PassportSeriesTextBox.Clear();
            PassportNumberTextBox.Clear();

            _selectedPhotoPath = null;
            _selectedPassportPdfPath = null;

            PhotoPathTextBlock.Text = "Фото не выбрано";
            PassportPdfPathTextBlock.Text = "PDF не выбран";
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void PhoneTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_isUpdatingPhone)
                return;

            _isUpdatingPhone = true;

            string digits = ExtractPhoneDigits(PhoneTextBox.Text);
            PhoneTextBox.Text = FormatPhone(digits);
            PhoneTextBox.CaretIndex = PhoneTextBox.Text.Length;

            _isUpdatingPhone = false;
        }

        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back && PhoneTextBox.CaretIndex <= 4)
            {
                e.Handled = true;
            }
        }

        private static string ExtractPhoneDigits(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }

        private static string FormatPhone(string digits)
        {
            if (string.IsNullOrEmpty(digits))
                return "+7 (";

            if (digits.StartsWith("8"))
                digits = "7" + digits.Substring(1);

            if (!digits.StartsWith("7"))
                digits = "7" + digits;

            if (digits.Length > 11)
                digits = digits.Substring(0, 11);

            string result = "+7 (";

            string local = digits.Length > 1 ? digits.Substring(1) : "";

            if (local.Length >= 1)
                result += local.Substring(0, Math.Min(3, local.Length));

            if (local.Length >= 3)
                result += ") ";

            if (local.Length > 3)
                result += local.Substring(3, Math.Min(3, local.Length - 3));

            if (local.Length >= 6)
                result += "-";

            if (local.Length > 6)
                result += local.Substring(6, Math.Min(2, local.Length - 6));

            if (local.Length >= 8)
                result += "-";

            if (local.Length > 8)
                result += local.Substring(8, Math.Min(2, local.Length - 8));

            return result;
        }

        private static bool IsPhotoAspectRatioThreeByFour(string path)
        {
            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BitmapDecoder decoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            BitmapFrame frame = decoder.Frames[0];

            double width = frame.PixelWidth;
            double height = frame.PixelHeight;

            if (width == 0 || height == 0)
                return false;

            double ratio = width / height;
            double expected = 3.0 / 4.0;

            return Math.Abs(ratio - expected) < 0.03;
        }
    }
}