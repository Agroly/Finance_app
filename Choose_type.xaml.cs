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
    /// <summary>
    /// Логика взаимодействия для Choose_type.xaml
    /// </summary>
    public partial class Choose_type : Window
    {
        public User currentuser;
        public Choose_type(User currentuser)
        {
            InitializeComponent();
            this.currentuser = currentuser;
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
                        DebitCardWindow debitCardWindow = new DebitCardWindow(currentuser);
                        debitCardWindow.ShowDialog();
                        break;

                    case "Кредитная карта":
                        CreditCardWindow creditCardWindow = new CreditCardWindow(currentuser);
                        creditCardWindow.ShowDialog();
                        break;
                    case "Накопительный счет":
                        SavingAccountWindow savingAccountWindow = new SavingAccountWindow(currentuser);
                        savingAccountWindow.ShowDialog();
                        break;
                    // Добавьте обработку других типов счетов здесь, если необходимо

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
