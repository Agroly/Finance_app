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
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private MainWindow mainWindow;
        public LoginWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
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
            // Строка подключения к базе данных PostgreSQL
            string connectionString = "Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем SQL-запрос для проверки пользователя
                    string selectQuery = "SELECT Username, LastName, FirstName, MiddleName, Age FROM Users WHERE Username = @Username AND Password = @Password";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            // Проверяем, есть ли данные
                            if (reader.Read())
                            {
                                // Извлекаем данные из результата запроса и создаем объект User
                                string lastName = reader["LastName"].ToString();
                                string firstName = reader["FirstName"].ToString();
                                string middleName = reader["MiddleName"].ToString();
                                int age = Convert.ToInt32(reader["Age"]);
                                MessageBox.Show("Вход успешен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                                User currentuser = new User(username, lastName, firstName, middleName, age);
                                WorkplaceWindow workplaceWindow = new WorkplaceWindow(currentuser);
                                workplaceWindow.Show();
                                mainWindow.Close();
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок подключения к базе данных
                MessageBox.Show($"Ошибка при проверке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
