using System.Windows;
using WpfApp1;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(this);
            loginWindow.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр нового окна регистрации
            RegistrationWindow registrationWindow = new RegistrationWindow();

            // Открываем окно регистрации
            registrationWindow.ShowDialog();
        }

    }
}
