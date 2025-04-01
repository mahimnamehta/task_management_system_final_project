namespace TaskManagerApp.Models
{
    public class LoginRequest
    {
        public LoginRequest(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public string email { get; set; }
        public string password { get; set; }
    }
}
