﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1
{
    public partial class WorkplaceWindow : Window
    {
        private User currentUser;

        public ObservableCollection<AccountViewModel> AccountViewModels { get; set; }

        public WorkplaceWindow(User СurrentUser)
        {
            InitializeComponent();
            this.currentUser = СurrentUser;
            DataContext = this;

            // Инициализируем коллекцию для отображения счетов
            AccountViewModels = new ObservableCollection<AccountViewModel>();

            // Инициализируем DataContext перед вызовом LoadAndDisplayAccounts
            DataContext = this;

            // Загружаем и отображаем счета текущего пользователя
            LoadAndDisplayAccounts();
        }

        private void LoadAndDisplayAccounts()
        {
            // Получаем счета пользователя из базы данных
            List<Account> userAccounts = GetAccountsForUser(User.GetUserIdByUsername(currentUser.Username));
            
            AccountViewModels.Clear();

            // Создаем коллекцию для отображения в ListBox
            foreach (var account in userAccounts)
            {
                AccountViewModels.Add(new AccountViewModel(account));
            }
        }

        // Метод для получения счетов пользователя из базы данных
        private List<Account> GetAccountsForUser(int userId)
        {
            return Database.GetAccountsForUser(userId);
        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Choose_type choose_Type = new Choose_type(currentUser);
            choose_Type.ShowDialog();

            // После добавления счета перезагружаем и отображаем счета
            LoadAndDisplayAccounts();
        }
        private void AddBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выбранный счет из ListBox
            AccountViewModel selectedAccountViewModel = (AccountViewModel)lstAccounts.SelectedItem;

            if (selectedAccountViewModel != null)
            {
                // Здесь передаем выбранный счет в метод, который обработает пополнение баланса
                 TopUpBalance window = new TopUpBalance(selectedAccountViewModel);
                 window.ShowDialog();
                 LoadAndDisplayAccounts();
            }
            else
            {
                MessageBox.Show("Выберите счет для пополнения баланса.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

}

