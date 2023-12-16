using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SavingAccountWindow : Window
    {
        private User currentuser;

        public SavingAccountWindow(User currentUser)
        {
            InitializeComponent();
            this.currentuser = currentUser;
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
                SavingAccount savingAccount = new SavingAccount(balance, procent);
                savingAccount.AddSavingAccount(User.GetUserIdByUsername(currentuser.Username));

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
