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