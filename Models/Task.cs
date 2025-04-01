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

        // Constructor to initialize non-nullable properties
        public Task(string id, string title, string description, DateTime dueDate, string category)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            Category = category;
            IsCompleted = false;  // Default value
        }
    }
}
