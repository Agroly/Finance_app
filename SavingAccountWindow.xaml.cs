using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SavingAccountWindow : Window
    {
        private User currentuser;
        private Database database;
        public SavingAccount savingAccount;

        public SavingAccountWindow(User currentUser, Database database)
        {
            InitializeComponent();
            this.currentuser = currentUser;
            this.database = database;
        }

        private void RemoveText(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = string.Empty;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем значения из полей ввода
                decimal balance = decimal.Parse(txtBalance.Text); // Здесь можно установить начальный баланс
                decimal procent = decimal.Parse(txtProcent.Text);

                // Здесь создаем экземпляр SavingAccount и добавляем его в базу данных
                savingAccount = new SavingAccount(balance, database, procent);
                database.AddSavingAccount(database.GetUserIdByUsername(currentuser.Username), savingAccount);

                MessageBox.Show("Накопительный счет успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении накопительного счета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
