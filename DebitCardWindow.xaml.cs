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
    public partial class DebitCardWindow : Window
    {
        private User currentuser;
        public DebitCardWindow(User CurrentUser)
        {
            InitializeComponent(); 
            this.currentuser = CurrentUser;
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

                // Проверяем, что все обязательные поля заполнены
                if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(cvv) || string.IsNullOrEmpty(paymentSystem) || string.IsNullOrEmpty(expiryDate))
                {
                    MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Здесь создаем экземпляр DebitCard и добавляем его в базу данных
                DebitCard debitCard = new DebitCard(0, cvv, cardNumber, paymentSystem, expiryDate);
                debitCard.AddDebitCard(User.GetUserIdByUsername(currentuser.Username));

                MessageBox.Show("Дебетовая карта успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении дебетовой карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}