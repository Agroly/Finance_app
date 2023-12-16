using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Account
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string OwnerName { get; set; }
        public decimal Balance { get; set; }

        public Account(int accountId, string accountNumber, string ownerName, decimal balance)
        {
            AccountId = accountId;
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            Balance = balance;
        }
    }
}
