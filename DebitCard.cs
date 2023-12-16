using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class DebitCard : Account
    {
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public string PaymentSystem { get; set; }
        public DateTime ExpiryDate { get; set; }

        public DebitCard(int accountId, string accountNumber, string ownerName, decimal balance,
                         string cvv, string cardNumber, string paymentSystem, DateTime expiryDate)
            : base(accountId, accountNumber, ownerName, balance)
        {
            CVV = cvv;
            CardNumber = cardNumber;
            PaymentSystem = paymentSystem;
            ExpiryDate = expiryDate;
        }
    }
}
