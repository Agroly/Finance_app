using Npgsql;
using System;
using System.Text.Json;
using System.Windows;

namespace WpfApp1
{
    public class CreditCard : DebitCard
    {
        public decimal Procent { get; set; }

        public CreditCard(decimal balance, Database db, string cvv, string cardNumber, string paymentSystem, string expiryDate, decimal procent)
            : base(balance,db, cvv, cardNumber, paymentSystem, expiryDate)
        {
            AccountTypeId = 2; 
            Procent = procent;
        }
        private JsonDocument ToJson()
        {
            // Используем библиотеку System.Text.Json для сериализации объекта в JSON

            var json = JsonSerializer.Serialize(new
            {
                CVV,
                CardNumber,
                PaymentSystem,
                ExpiryDate,
                Procent
            });
            return JsonDocument.Parse(json); ;
        }
    }
}
