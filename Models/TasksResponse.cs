
namespace TaskManagerApp.Models
{
    public class TasksResponse
    {
        public List<TaskItem> AllTasks { get; set; }
        public string Error { get; set; }

        public class TaskItem
        {
            public string AssignedToName { get; set; }
            public string AssignedToUid { get; set; }
            public string CreatedByName { get; set; }
            public string CreatedByUid { get; set; }
            public string Description { get; set; }
            public bool Done { get; set; }
            public string TaskUid { get; set; }
        }
    }
}
