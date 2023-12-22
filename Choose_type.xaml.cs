using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Choose_type : Window
    {
        public User currentuser;
        private Database db;
        private ObservableCollection<AccountViewModel> AccountViewModels { get; set; }
        public Choose_type(User currentuser, Database db, ObservableCollection<AccountViewModel> accountViewModels)
        {
            InitializeComponent();
            this.currentuser = currentuser;
            this.db = db;
            AccountViewModels = accountViewModels;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем выбранный тип счета
                string selectedAccountType = ((ComboBoxItem)cmbAccountType.SelectedItem)?.Content?.ToString();

                // Проверяем, что тип счета выбран
                if (string.IsNullOrEmpty(selectedAccountType))
                {
                    MessageBox.Show("Выберите тип счета!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // В зависимости от выбранного типа счета открываем соответствующее окно
                switch (selectedAccountType)
                {
                    case "Дебетовая карта":
                        DebitCardWindow debitCardWindow = new DebitCardWindow(currentuser, db);
                        debitCardWindow.ShowDialog();
                        if(debitCardWindow.finished)
                        AccountViewModels.Add(new AccountViewModel(debitCardWindow.debitCard));
                        break;

                    case "Кредитная карта":
                        CreditCardWindow creditCardWindow = new CreditCardWindow(currentuser, db);
                        creditCardWindow.ShowDialog();
                        if(creditCardWindow.finished)
                        AccountViewModels.Add(new AccountViewModel(creditCardWindow.creditCard));
                        break;
                    case "Накопительный счет":
                        SavingAccountWindow savingAccountWindow = new SavingAccountWindow(currentuser, db);
                        savingAccountWindow.ShowDialog();
                        if(savingAccountWindow.finished)
                        AccountViewModels.Add(new AccountViewModel(savingAccountWindow.savingAccount));
                        break;

                    default:
                        MessageBox.Show("Неизвестный тип счета!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
