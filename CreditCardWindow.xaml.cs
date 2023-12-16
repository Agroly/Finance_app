﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class CreditCardWindow : Window
    {
        private User currentuser;

        public CreditCardWindow(User currentUser)
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
                CreditCard creditCard = new CreditCard(0, cvv, cardNumber, paymentSystem, expiryDate, procent);
                creditCard.AddCreditCard(User.GetUserIdByUsername(currentuser.Username));

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