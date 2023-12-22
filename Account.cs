namespace WpfApp1
{
    public class Account
    {
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        public int AccountId { get; set; }

        public Database db { get; set; } 
          

        public Account(decimal balance, Database database)
        {
            Balance = balance;
            db = database;
        }
        public Account()
        {
 
        }

        
    }
}


