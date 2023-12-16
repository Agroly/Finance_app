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
    /// Логика взаимодействия для WorkplaceWindow.xaml
    /// </summary>
    public partial class WorkplaceWindow : Window
    {
        private User currentUser;
        public WorkplaceWindow(User СurrentUser)
        {
            InitializeComponent();
            this.currentUser = СurrentUser;
            DataContext = this;
        }
        public string GreetingText
        {
            get { return $"Здравствуйте, {currentUser.FirstName} {currentUser.LastName}!"; }
        }
        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь добавьте код для открытия окна добавления счета
            // Например, вы можете создать новое окно или использовать существующее
            // и передать текущего пользователя
            
        }

    }
}
