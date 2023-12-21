using System;
using System.Windows;

namespace WpfApp1
{
    public partial class TransferDialog : Window
    {
        public decimal TransferAmount { get; private set; }

        public AccountViewModel Sender { get; set; }

        public AccountViewModel Receiver { get; set; }
        private Database db; 

        public TransferDialog(AccountViewModel Sender, AccountViewModel Reciever, Database db)
        {
            InitializeComponent();
            this.Sender = Sender;
            this.Receiver = Reciever;
            this.db = db;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Попробуем преобразовать введенное значение в decimal
            if (decimal.TryParse(txtTransferAmount.Text, out decimal amount))
            {
                TransferAmount = amount;
                db.TransferFunds(Sender.Account, Receiver.Account, amount);
                DialogResult = true; // Устанавливаем DialogResult в true, чтобы указать успешное завершение диалога
            }
            else
            {
                MessageBox.Show("Введите корректную сумму перевода.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Устанавливаем DialogResult в false, чтобы указать отмену диалога
        }
    }
}
