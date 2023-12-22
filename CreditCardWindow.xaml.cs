using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class CreditCardWindow : Window
    {
        private User currentuser;
        private Database db;
        public CreditCard creditCard;
        public bool finished = false;

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
        private bool IsValidCVV(string cvv)
        {
            // Проверка, что CVV состоит из трех цифр
            Regex regex = new Regex(@"^\d{3}$");
            return regex.IsMatch(cvv);
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            // Проверка, что номер карты состоит из 16 цифр
            Regex regex = new Regex(@"^\d{16}$");
            return regex.IsMatch(cardNumber);
        }

        private bool IsValidExpirationDate(string expirationDate)
        {
            // Проверка, что дата истечения срока действия имеет формат MM/YY
            Regex regex = new Regex(@"^(0[1-9]|1[0-2])\/\d{2}$");
            if (!regex.IsMatch(expirationDate))
            {
                return false;
            }
            string[] parts = expirationDate.Split('/');
            if (parts.Length == 2)
            {
                int month, year;
                if (int.TryParse(parts[0], out month) && int.TryParse(parts[1], out year))
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime expiration = new DateTime(2000 + year, month, DateTime.DaysInMonth(2000 + year, month));
                    return expiration > currentDate;
                }
            }

            return false;
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
                

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvv) || string.IsNullOrEmpty(paymentSystem) || string.IsNullOrEmpty(expiryDate))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!IsValidCVV(txtCVV.Text))
                {
                    MessageBox.Show("CVV должен содержать три цифры.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверяем корректность ввода номера карты
                if (!IsValidCardNumber(txtCardNumber.Text))
                {
                    MessageBox.Show("Номер карты должен содержать 16 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверяем корректность ввода даты истечения срока действия
                if (!IsValidExpirationDate(expiryDate))
                {
                    MessageBox.Show("Дата истечения срока действия должна иметь формат MM/YY и быть не просроченной.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!decimal.TryParse(txtProcent.Text, out decimal procent) || procent < 0 || procent > 100)
                {
                    MessageBox.Show("Введите корректное значение для процентной ставки (число от 0 до 100).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Здесь создаем экземпляр CreditCard и добавляем его в базу данных
                creditCard = new CreditCard(0, db, cvv, cardNumber, paymentSystem, expiryDate, procent);
                db.AddCreditCard(currentuser.UserId,creditCard);

                MessageBox.Show("Кредитная карта успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                finished = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении кредитной карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
