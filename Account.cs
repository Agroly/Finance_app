using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class Account
    {
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }

        public int AccountId { get; set; }

          

        public Account(decimal balance)
        {
            Balance = balance;
        }
        public Account()
        {
 
        }

        public void AddAccount(int UserId)
        {
            // Строка подключения к базе данных PostgreSQL
            string connectionString = "Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем SQL-запрос для добавления счета
                    string insertQuery = "INSERT INTO Accounts (Balance, UserId) VALUES (@Balance, @UserId) RETURNING AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@Balance", Balance);
                        command.Parameters.AddWithValue("@UserId", UserId);
                        AccountId = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении счета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }
    }
}


