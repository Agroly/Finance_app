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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для TopUpBalance.xaml
    /// </summary>
    public partial class TopUpBalance : Window
    {
        private AccountViewModel account;
        private Database db;
        public TopUpBalance(AccountViewModel selectedaccount, Database db)
        {
            InitializeComponent();
            this.account = selectedaccount;
            this.db = db;
           
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем введенную сумму
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                // Вызываем метод для пополнения баланса аккаунта
                bool success = db.TopUpBalanceByAccount(account.Account.AccountId, amount);

                if (success)
                {
                    MessageBox.Show($"Баланс успешно пополнен на {amount:C2}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось выполнить операцию. Пожалуйста, попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите корректное число для пополнения баланса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
