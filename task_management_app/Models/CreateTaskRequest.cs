namespace task_management_app.Models
{
    public class CreateTaskRequest
    {
        public string assignedToUid { get; set; }
        public string description { get; set; }
    }
}
