namespace task_management_app.Models
{
    public class AllUsersResponse
    {
        public List<User> allUsers {  get; set; }

        public class User
        {
            public string uid { get; set; }
            public string email { get; set; }
            public string name { get; set; }
        }
    }
}
