using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1
{
    public partial class WorkplaceWindow : Window
    {
        private User currentUser;

        public ObservableCollection<AccountViewModel> AccountViewModels { get; set; }

        public AccountViewModel Sender { get; set; }

        public AccountViewModel Receiver { get; set; }
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
        private void SelectSenderButton_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем выбранный аккаунт как отправителя
            AccountViewModel selectedAccount = (AccountViewModel)lstAccounts.SelectedItem;

            if (selectedAccount != null)
            {
                // Проверяем, что выбранный аккаунт не равен текущему получателю
                if (Receiver == null || selectedAccount != Receiver)
                {
                    Sender = selectedAccount;
                    MessageBox.Show($"Выбран отправитель: {Sender.Details}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выберите другой аккаунт в качестве отправителя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите счет для установки отправителя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SelectReceiverButton_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем выбранный аккаунт как получателя
            AccountViewModel selectedAccount = (AccountViewModel)lstAccounts.SelectedItem;

            if (selectedAccount != null)
            {
                // Проверяем, что выбранный аккаунт не равен текущему отправителю
                if (Sender == null || selectedAccount != Sender)
                {
                    Receiver = selectedAccount;
                    MessageBox.Show($"Выбран получатель: {Receiver.Details}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Выберите другой аккаунт в качестве получателя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Выберите счет для установки получателя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбраны ли отправитель и получатель
            if (Sender != null && Receiver != null)
            {
                // Создаем диалог для ввода суммы перевода
                TransferDialog transferDialog = new TransferDialog(this.Sender, this.Receiver);

                // Показываем диалог и получаем результат
                bool? result = transferDialog.ShowDialog();
                if (result == true) { LoadAndDisplayAccounts(); }
            }


            else
            {
                MessageBox.Show("Выберите отправителя и получателя перед выполнением перевода.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


    }

}

