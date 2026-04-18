using System.Windows;

namespace KeeperProWpf.Department.Windows
{
    public partial class BlackListReasonWindow : Window
    {
        public string ReasonText { get; private set; } = "";

        public BlackListReasonWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReasonTextBox.Text))
            {
                MessageBox.Show("Введите причину.");
                return;
            }

            ReasonText = ReasonTextBox.Text.Trim();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}