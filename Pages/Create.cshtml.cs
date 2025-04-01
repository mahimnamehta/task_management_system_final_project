using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly MongoDbContext _dbContext;

        public CreateModel(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public DateTime DueDate { get; set; }

        [BindProperty]
        public string AssignedToUid { get; set; }

        public string ErrorMessage { get; set; }
        public List<User> Users { get; set; } = new List<User>();

        public async Task<IActionResult> OnGet()
        {
            try
            {
                // Fetch users from MongoDB for assignment
                Users = await _dbContext.Users.Find(user => true).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error fetching users from MongoDB: " + ex.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                // Generate a new task ID (GUID or other approach)
                string taskId = Guid.NewGuid().ToString();

                // Get the current user's ID from the session (this should be from your session management)
                var createdByUid = GetToken(); // Assuming the token stores the user ID

                if (string.IsNullOrEmpty(createdByUid))
                {
                    ErrorMessage = "You must be logged in to create a task.";
                    return Page();
                }

                // Create a new Task object
                var task = new TaskManagerApp.Models.Task(
                    taskId,
                    Title,
                    Description,
                    DueDate,
                    Category,
                    createdByUid, // Set the CreatedByUid
                    AssignedToUid // Set the AssignedToUid
                );

                // Insert the task into MongoDB
                await _dbContext.Tasks.InsertOneAsync(task);

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                // Handle errors
                ErrorMessage = "Error creating task in MongoDB: " + ex.Message;
            }

            return Page();
        }

        private string GetToken()
        {
            // Retrieve the user ID from session (or JWT token, depending on your auth system)
            return HttpContext.Session.GetString("auth_token"); // This assumes the user ID is stored in the session token
        }
    }
}
