using Npgsql;
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
    public partial class RegistrationWindow : Window
    {
        private Database db;
        public RegistrationWindow(Database db)
        {
            InitializeComponent();
            this.db = db;
        }
        private void RemoveText(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.TextBox)sender).Text = string.Empty;
        }
        private void RemoveTextPassword(object sender, RoutedEventArgs e)
        {
            TextBox placeholder = (TextBox)sender;
            placeholder.Visibility = Visibility.Collapsed;
            txtPassword.Focus();
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем значения из полей ввода
            User newUser = new User(txtUsername.Text, txtPassword.Password, txtLastName.Text, txtFirstName.Text, txtMiddleName.Text);


            if (string.IsNullOrEmpty(newUser.Username) || string.IsNullOrEmpty(newUser.Password) ||
                string.IsNullOrEmpty(newUser.LastName) || string.IsNullOrEmpty(newUser.FirstName))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем, что возраст не пустой
            if (string.IsNullOrEmpty(txtAge.Text))
            {
                MessageBox.Show("Введите возраст!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Пытаемся преобразовать возраст в число
            if (int.TryParse(txtAge.Text, out int ageValue))
            {
                newUser.Age = ageValue;
            }
            else
            {
                MessageBox.Show("Некорректный возраст! Введите числовое значение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Выполняем регистрацию
            if (db.RegisterUser(newUser))
            {
                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Закрываем окно регистрации после успешной регистрации
            }
            else
            {
                MessageBox.Show("Не удалось выполнить регистрацию. Пожалуйста, попробуйте снова.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
