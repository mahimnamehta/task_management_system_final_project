namespace TaskManagerApp.Models
{
    public class Task
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public string Category { get; set; }

        // Add CreatedByUid to track the user who created the task
        public string CreatedByUid { get; set; } // Created by user ID

        public string AssignedToUid { get; set; } // Assigned user ID

        // Constructor to initialize non-nullable properties
        public Task(string id, string title, string description, DateTime dueDate, string category, string createdByUid, string assignedToUid)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Category = category;
            CreatedByUid = createdByUid; // Initialize CreatedByUid
            AssignedToUid = assignedToUid; // Initialize AssignedToUid
            IsCompleted = false;  // Default value
        }
    }
}
