using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class DebitCard : Account
    {
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public string PaymentSystem { get; set; }
        public string ExpiryDate { get; set; }

        public DebitCard(decimal balance, string cvv, string cardNumber, string paymentSystem, string expiryDate)
            : base(balance)
        {
            AccountTypeId = 1;
            CVV = cvv;
            CardNumber = cardNumber;
            PaymentSystem = paymentSystem;
            ExpiryDate = expiryDate;
        }
        private JsonDocument ToJson()
        {
            // Используем библиотеку System.Text.Json для сериализации объекта в JSON
            var json = JsonSerializer.Serialize(new
            {
                CVV,
                CardNumber,
                PaymentSystem,
                ExpiryDate
            });

            return JsonDocument.Parse(json); ;
        }
        public void AddDebitCard(int userId)
        {
            AddAccount(userId);
            // Вызываем метод добавления счета
            int accountId = AccountId;
            JsonDocument jsonParams = this.ToJson();

            // Строка подключения к базе данных PostgreSQL
            string connectionString = "Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем SQL-запрос для обновления поля AccountParams
                    string updateQuery = "UPDATE Accounts SET AccountParams = @AccountParams, AccountTypeId = @AccountTypeId WHERE AccountId = @AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@AccountParams", jsonParams);
                        command.Parameters.AddWithValue("@AccountTypeId", AccountTypeId);
                        command.Parameters.AddWithValue("@AccountId", accountId);

                        // Выполняем запрос
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении AccountParams: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}