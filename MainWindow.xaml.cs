using System.Windows;
using WpfApp1;

namespace YourNamespace
{
    public partial class MainWindow : Window
    {
        public Database db { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            db = new Database();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(this,db);
            loginWindow.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем экземпляр нового окна регистрации
            RegistrationWindow registrationWindow = new RegistrationWindow(db);

            // Открываем окно регистрации
            registrationWindow.ShowDialog();
        }

    }
}
