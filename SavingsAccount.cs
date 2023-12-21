using Npgsql;
using System;
using System.Text.Json;
using System.Windows;

namespace WpfApp1
{
    public class SavingAccount : Account
    {
        public decimal Procent { get; set; }

        public SavingAccount(decimal balance, Database db, decimal procent)
            : base(balance, db)
        {
            AccountTypeId = 3;
            Procent = procent;
        }
        public JsonDocument ToJson()
        {
            // Используем библиотеку System.Text.Json для сериализации объекта в JSON

            var json = JsonSerializer.Serialize(new
            {
                Procent
            });
            return JsonDocument.Parse(json); ;
        }
       
    }

}
