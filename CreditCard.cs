using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class CreditCard : Account
    {
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public string PaymentSystem { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal InterestRate { get; set; }
        public decimal StartBalance { get; set; }

        public CreditCard(int accountId, string accountNumber, string ownerName, decimal balance,
                          string cvv, string cardNumber, string paymentSystem, DateTime expiryDate,
                          decimal interestRate, decimal startBalance)
            : base(accountId, accountNumber, ownerName, balance)
        {
            CVV = cvv;
            CardNumber = cardNumber;
            PaymentSystem = paymentSystem;
            ExpiryDate = expiryDate;
            InterestRate = interestRate;
            StartBalance = startBalance;
        }
    }
}
