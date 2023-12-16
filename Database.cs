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
    }
}
