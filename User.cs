using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int Age { get; set; }
        public User(string username, string lastName, string firstName, string middleName, int age)
        {
            Username = username;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            Age = age;
        }
        public User(string username, string password, string lastName, string firstName, string middleName)
        {
            Username = username;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
        }
        public static int GetUserIdByUsername(string username)
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = "Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем SQL-запрос для получения Id пользователя по логину
                    string selectQuery = "SELECT Id FROM Users WHERE Username = @Username";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@Username", username);

                        // Выполняем SQL-запрос и получаем результат
                        object result = command.ExecuteScalar();

                        // Если результат не null, преобразуем его в int и возвращаем
                        if (result != null && int.TryParse(result.ToString(), out int userId))
                        {
                            return userId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок подключения к базе данных
                MessageBox.Show($"Ошибка при получении Id пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return -1; // Если пользователя не найдено или произошла ошибка
        }
    }

}
