using Npgsql;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;


namespace WpfApp1
{
    public class Database
    { 
        private static readonly NpgsqlConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=xaethei7raiTeeso;Database=kursach;Port=5432;");

        public Database()
        {
            connection.Open(); // Открываем подключение при создании экземпляра класса
        }

        public User AuthenticateUser(string username, string password)
        {
            try
            {
                // Создаем SQL-запрос для проверки пользователя
                string selectQuery = "SELECT Id, Username, LastName, FirstName, MiddleName, Age FROM Users WHERE Username = @Username AND Password = @Password";

                // Создаем команду с использованием открытого подключения
                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, connection))
                {
                    // Передаем параметры в запрос
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    // Выполняем запрос и используем reader
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        // Проверяем, есть ли данные
                        if (reader.Read())
                        {
                            // Извлекаем данные из результата запроса и создаем объект User
                            int userId = Convert.ToInt32(reader["Id"]);
                            string lastName = reader["LastName"].ToString();
                            string firstName = reader["FirstName"].ToString();
                            string middleName = reader["MiddleName"].ToString();
                            int age = Convert.ToInt32(reader["Age"]);

                            User currentUser = new User( username, lastName, firstName, middleName, age);
                            currentUser.UserId = userId;

                            // Закрываем reader после использования
                            reader.Close();

                            return currentUser;
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                            // Закрываем reader после использования
                            reader.Close();

                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок подключения к базе данных
                MessageBox.Show($"Ошибка при проверке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        public int AddAccount(int UserId, Account account)
        {
            try
            {
                    // Создаем SQL-запрос для добавления счета
                    string insertQuery = "INSERT INTO Accounts (Balance, UserId) VALUES (@Balance, @UserId) RETURNING AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@Balance", account.Balance);
                        command.Parameters.AddWithValue("@UserId", UserId);
                        return (int)command.ExecuteScalar();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении счета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }
        public List<Account> GetAccountsForUser(int userId)
        {
            List<Account> accounts = new List<Account>();
            try
                {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запросе счетов: {ex.Message}");
            }
            return accounts;
        }
        public bool TopUpBalanceByAccount(int accountId, decimal amount)
        {
            try
            {
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
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при пополнении баланса в базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Если возникла ошибка, возвращаем false
            return false;
        }
        public bool TransferFunds(Account senderAccount, Account recipientAccount, decimal amount)
        {
            // Проверяем, что у отправителя достаточно средств для перевода
            if (senderAccount.Balance >= amount)
            {
                // Уменьшаем баланс отправителя
                bool senderSuccess = TopUpBalanceByAccount(senderAccount.AccountId, -amount);

                if (senderSuccess)
                {
                    // Если уменьшение баланса отправителя прошло успешно,
                    // увеличиваем баланс получателя
                    bool recipientSuccess = TopUpBalanceByAccount(recipientAccount.AccountId, amount);

                    // Если увеличение баланса получателя также успешно,
                    // возвращаем true
                    if (recipientSuccess)
                        return true;
                    else
                    {
                        // В случае неудачи при увеличении баланса получателя,
                        // восстанавливаем баланс отправителя
                        TopUpBalanceByAccount(senderAccount.AccountId, amount);
                    }
                }
            }

            // Если что-то пошло не так или у отправителя недостаточно средств,
            // возвращаем false
            return false;
        }
        public bool RegisterUser(User user)
        {
            try
            {
                    // Создаем SQL-запрос для вставки нового пользователя
                    string insertQuery = "INSERT INTO Users (Username, Password, LastName, FirstName, MiddleName, Age) " +
                                         "VALUES (@Username, @Password, @LastName, @FirstName, @MiddleName, @Age)";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@MiddleName", user.MiddleName);
                        command.Parameters.AddWithValue("@Age", user.Age);

                        // Выполняем SQL-запрос и возвращаем результат
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                        
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
       
        public void AddDebitCard(int userId, DebitCard account)
        {
            int accountId = AddAccount(userId, account);
            // Вызываем метод добавления счета
            JsonDocument jsonParams = account.ToJson();
            try
            {
                    // Создаем SQL-запрос для обновления поля AccountParams
                    string updateQuery = "UPDATE Accounts SET AccountParams = @AccountParams, AccountTypeId = @AccountTypeId WHERE AccountId = @AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        // Передаем параметры в запрос
                        command.Parameters.AddWithValue("@AccountParams", jsonParams);
                        command.Parameters.AddWithValue("@AccountTypeId", account.AccountTypeId);
                        command.Parameters.AddWithValue("@AccountId", accountId);

                        // Выполняем запрос
                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении AccountParams: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void AddSavingAccount(int userId, SavingAccount account)
        {
            int accountId = AddAccount(userId, account);

            JsonDocument jsonParams = account.ToJson();
            try
            {
                    string updateQuery = "UPDATE Accounts SET AccountParams = @AccountParams, AccountTypeId = @AccountTypeId WHERE AccountId = @AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AccountParams", jsonParams);
                        command.Parameters.AddWithValue("@AccountTypeId", account.AccountTypeId);
                        command.Parameters.AddWithValue("@AccountId", accountId);

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении AccountParams: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void AddCreditCard(int userId, CreditCard account)
        {
            int accountId = AddAccount(userId, account);

            JsonDocument jsonParams = account.ToJson();

            try
            {
                    string updateQuery = "UPDATE Accounts SET AccountParams = @AccountParams, AccountTypeId = @AccountTypeId WHERE AccountId = @AccountId";

                    using (NpgsqlCommand command = new NpgsqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@AccountParams", jsonParams);
                        command.Parameters.AddWithValue("@AccountTypeId", account.AccountTypeId);
                        command.Parameters.AddWithValue("@AccountId", accountId);

                        command.ExecuteNonQuery();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении AccountParams: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

