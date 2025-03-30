namespace task_management_app.Models
{
    public class TasksResponse
    {
        public List<TaskItem> allTasks { get; set; }
        public string error { get; set; }

        public class TaskItem {
            public string assignedToName { get; set; }
            public string assignedToUid { get; set; }
            public string createdByName { get; set; }
            public string createdByUid { get; set; }
            public string description { get; set; }
            public bool done { get; set; }
            public string taskUid { get; set; }
        }
    }
}