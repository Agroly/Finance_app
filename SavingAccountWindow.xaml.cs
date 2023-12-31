﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class SavingAccountWindow : Window
    {
        private User currentuser;
        private Database database;
        public SavingAccount savingAccount;
        public bool finished = false;

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
                if (!decimal.TryParse(txtProcent.Text, out decimal procent) || procent < 0 || procent > 100)
                {
                    MessageBox.Show("Введите корректное значение для процентной ставки (число от 0 до 100).", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Получаем значения из полей ввода
                decimal balance = decimal.Parse(txtBalance.Text); // Здесь можно установить начальный баланс

                // Здесь создаем экземпляр SavingAccount и добавляем его в базу данных
                savingAccount = new SavingAccount(balance, database, procent);
                database.AddSavingAccount(currentuser.UserId, savingAccount);

                MessageBox.Show("Накопительный счет успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                finished = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении накопительного счета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
