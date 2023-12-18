using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class Database
    {
        private static string connectionString = "Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;";

        public static List<Account> GetAccountsForUser(int userId)
        {
            List<Account> accounts = new List<Account>();


            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем SQL-запрос для получения счетов пользователя
                    string selectQuery = "SELECT * FROM accounts WHERE UserId = @UserId";

                    using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                // Создаем экземпляр счета и добавляем его в список
                                Account account = new Account()
                                {
                                    AccountId = Convert.ToInt32(reader["AccountId"]),
                                    Balance = Convert.ToDecimal(reader["Balance"]),
                                    AccountTypeId = Convert.ToInt32(reader["AccountTypeId"]),
                                };
                                

                                accounts.Add(account);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе счетов: {ex.Message}");
            }

            return accounts;
        }
        public static bool TopUpBalanceByAccount(int accountId, decimal amount)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создаем SQL-запрос для обновления баланса счета по его ID
                    string updateQuery = "UPDATE accounts SET Balance = Balance + @Amount WHERE AccountId = @AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@AccountId", accountId);

                        // Выполняем запрос
                        int rowsAffected = command.ExecuteNonQuery();

                        // Если была хотя бы одна успешно обновленная строка, возвращаем true
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при пополнении баланса по ID: {ex.Message}");
            }

            // Если возникла ошибка, возвращаем false
            return false;
        }
    }
}
