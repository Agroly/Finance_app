using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class DebitCardWindow : Window
    {
        private User currentuser;
        private Database db;
        public DebitCard debitCard;
        public bool finished = false;
        public DebitCardWindow(User CurrentUser, Database db)
        {
            InitializeComponent(); 
            this.currentuser = CurrentUser;
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

                // Здесь создаем экземпляр DebitCard и добавляем его в базу данных
                debitCard = new DebitCard(0,db, cvv, cardNumber, paymentSystem, expiryDate);
                db.AddDebitCard(currentuser.UserId,debitCard);

                MessageBox.Show("Дебетовая карта успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                finished = true;
                this.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении дебетовой карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}