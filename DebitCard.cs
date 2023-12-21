using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class DebitCard : Account
    {
        public string CVV { get; set; }
        public string CardNumber { get; set; }
        public string PaymentSystem { get; set; }
        public string ExpiryDate { get; set; }

        public DebitCard(decimal balance, Database db, string cvv, string cardNumber, string paymentSystem, string expiryDate)
            : base(balance, db)
        {
            AccountTypeId = 1;
            CVV = cvv;
            CardNumber = cardNumber;
            PaymentSystem = paymentSystem;
            ExpiryDate = expiryDate;
        }
        public JsonDocument ToJson()
        {
            // Используем библиотеку System.Text.Json для сериализации объекта в JSON
            var json = JsonSerializer.Serialize(new
            {
                CVV,
                CardNumber,
                PaymentSystem,
                ExpiryDate
            });

            return JsonDocument.Parse(json); ;
        }
    }     
}