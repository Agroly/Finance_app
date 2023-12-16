﻿using Npgsql;
using System;
using System.Text.Json;
using System.Windows;

namespace WpfApp1
{
    public class CreditCard : DebitCard
    {
        public decimal Procent { get; set; }

        public CreditCard(decimal balance, string cvv, string cardNumber, string paymentSystem, string expiryDate, decimal procent)
            : base(balance, cvv, cardNumber, paymentSystem, expiryDate)
        {
            AccountTypeId = 2; 
            Procent = procent;
        }
        private JsonDocument ToJson()
        {
            // Используем библиотеку System.Text.Json для сериализации объекта в JSON

            var json = JsonSerializer.Serialize(new
            {
                CVV,
                CardNumber,
                PaymentSystem,
                ExpiryDate,
                Procent
            });
            return JsonDocument.Parse(json); ;
        }
        public void AddCreditCard(int userId)
        {
            AddAccount(userId);
            
            JsonDocument jsonParams = this.ToJson();

            string connectionString = "Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Accounts SET AccountParams = @AccountParams, AccountTypeId = @AccountTypeId WHERE AccountId = @AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AccountParams", jsonParams);
                        command.Parameters.AddWithValue("@AccountTypeId", AccountTypeId);
                        command.Parameters.AddWithValue("@AccountId", AccountId);

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