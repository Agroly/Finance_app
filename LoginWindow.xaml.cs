using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;
using YourNamespace;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        private MainWindow mainWindow;

        public Database database;
        public LoginWindow(MainWindow mainWindow, Database database)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.database = database;
        }
        public User CurrentUser { get; private set; } // Текущий пользователь
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.TextBox)sender).Text = string.Empty;

        }
        private void RemoveTextPassword(object sender, RoutedEventArgs e)
        {
            TextBox placeholder = (TextBox)sender;
            placeholder.Visibility = Visibility.Collapsed;
            txtPassword.Focus();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем значения из полей ввода
            string username = txtLogin.Text;
            string password = txtPassword.Password;

            // Выполняем вход
            AuthenticateUser(username, password);        
        }

        private void AuthenticateUser(string username, string password)
        {
            this.Close();
            this.CurrentUser = database.AuthenticateUser(username, password);
            if (this.CurrentUser != null)
            {
                WorkplaceWindow workplaceWindow = new WorkplaceWindow(CurrentUser, database);
                workplaceWindow.Show();
                mainWindow.Close();
            }
        }
    }
}
