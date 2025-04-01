namespace TaskManagerApp.Models
{
    public class SignUpRequest
    {

        public SignUpRequest(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }

        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
