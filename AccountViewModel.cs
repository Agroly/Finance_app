using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class AccountViewModel
    {
        public string Details { get; set; }
        public Account Account { get; set; }
        public AccountViewModel(Account account)
        {
            Details = $"Баланс: {account.Balance:C2}, Тип счета: {GetAccountTypeName(account.AccountTypeId)}, ID счёта: {account.AccountId}";
            Account = account ;
        }

        private string GetAccountTypeName(int accountTypeId)
        {
            if (accountTypeId == 1) return "Дебетовая карта";
            if (accountTypeId == 2) return "Кредитная карта";
            if (accountTypeId == 3) return "Накопительный счет";
            return "Unknown";
        }
    }
}
