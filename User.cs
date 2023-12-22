    namespace WpfApp1
    {
        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public int Age { get; set; }
            public int UserId { get; set; }
            public User(string username, string lastName, string firstName, string middleName, int age)
            {
                Username = username;
                LastName = lastName;
                FirstName = firstName;
                MiddleName = middleName;
                Age = age;
            }
            public User(string username, string password, string lastName, string firstName, string middleName)
            {
                Username = username;
                Password = password;
                LastName = lastName;
                FirstName = firstName;
                MiddleName = middleName;
            }
        
        }

    }
