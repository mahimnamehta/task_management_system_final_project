namespace TaskManagerApp.Models
{
    public class SignUpDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AssignedTo { get; set; }
        public string PasswordHash { get; internal set; }

        // Parameterless constructor
        public User() { }

        // Constructor with parameters (if needed)
        public User(string id, string username, string email, string assignedTo)
        {
            Id = id;
            Username = username;
            Email = email;
            AssignedTo = assignedTo;
        }
    }
}
