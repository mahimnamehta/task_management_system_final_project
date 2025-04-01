namespace TaskManagerApp.Models
{
    public class LoginResponse
    {
        public LoginResponse(string token, string error)
        {
            this.token = token;
            this.error = error;
        }

        public string token { get; set; }
        public string error { get; set; }
    }
}
