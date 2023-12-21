using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class CreditCardWindow : Window
    {
        private User currentuser;
        private Database db;
        public CreditCard creditCard;

        public CreditCardWindow(User currentUser, Database db)
        {
            InitializeComponent();
            this.currentuser = currentUser;
            this.db = db;
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
                string cardNumber = txtCardNumber.Text;
                string cvv = txtCVV.Text;
                string paymentSystem = txtPaymentSystem.Text;
                string expiryDate = txtExpiryDate.Text;
                decimal procent = decimal.Parse(txtProcent.Text); // Добавлено поле Процентная ставка

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvv) || string.IsNullOrEmpty(paymentSystem) || string.IsNullOrEmpty(expiryDate))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Здесь создаем экземпляр CreditCard и добавляем его в базу данных
                creditCard = new CreditCard(0, db, cvv, cardNumber, paymentSystem, expiryDate, procent);
                db.AddCreditCard(db.GetUserIdByUsername(currentuser.Username),creditCard);

                MessageBox.Show("Кредитная карта успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении кредитной карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
