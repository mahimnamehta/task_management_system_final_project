using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MongoDbContext _dbContext;

        public IndexModel(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TaskManagerApp.Models.Task> Tasks { get; set; } = new List<TaskManagerApp.Models.Task>();
        public bool isShowingCreatedBy = true;
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGet(string ErrorMessage, bool IsAssignedToTab)
        {
            this.ErrorMessage = ErrorMessage;
            return await (IsAssignedToTab ? OnGetAssignedToMe() : OnGetCreatedByMe());
        }

        // Fetch tasks created by the current user
        public async Task<IActionResult> OnGetCreatedByMe()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            Tasks = await FetchTasksAsync("CreatedByMe", token);
            isShowingCreatedBy = true;

            return Page();
        }

        // Fetch tasks assigned to the current user
        public async Task<IActionResult> OnGetAssignedToMe()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            Tasks = await FetchTasksAsync("AssignedToMe", token);
            isShowingCreatedBy = false;

            return Page();
        }

        public async Task<List<TaskManagerApp.Models.Task>> FetchTasksAsync(string filter, string token)
        {
            try
            {
                var filterDefinition = Builders<TaskManagerApp.Models.Task>.Filter.Empty;

                if (filter == "CreatedByMe")
                {
                    // Filter tasks created by the user (you might need a user ID or similar for filtering)
                    var userId = token; // Assuming `token` is the user ID, replace with actual logic to retrieve user ID from session
                    filterDefinition = Builders<TaskManagerApp.Models.Task>.Filter.Eq(t => t.CreatedByUid, userId);  // Use CreatedByUid for filtering
                }
                else if (filter == "AssignedToMe")
                {
                    // Filter tasks assigned to the user
                    var userId = token; // Assuming `token` is the user ID, replace with actual logic
                    filterDefinition = Builders<TaskManagerApp.Models.Task>.Filter.Eq(t => t.AssignedToUid, userId);
                }

                return await _dbContext.Tasks.Find(filterDefinition).ToListAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error fetching tasks from MongoDB: " + ex.Message;
                return new List<TaskManagerApp.Models.Task>();
            }
        }

        // Delete a task from MongoDB
        public async Task<IActionResult> OnPostDeleteTask(string taskUid)
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var deleteResult = await _dbContext.Tasks.DeleteOneAsync(t => t.Id == taskUid);
                if (deleteResult.DeletedCount > 0)
                {
                    return RedirectToPage();
                }
                else
                {
                    ErrorMessage = "Error deleting task from MongoDB.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error deleting task from MongoDB: " + ex.Message;
            }

            return RedirectToPage(new { ErrorMessage });
        }

        // Mark a task as completed (done) in MongoDB
        public async Task<IActionResult> OnPostMarkAsDone(string taskUid)
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var updateResult = await _dbContext.Tasks.UpdateOneAsync(
                    Builders<TaskManagerApp.Models.Task>.Filter.Eq(t => t.Id, taskUid),
                    Builders<TaskManagerApp.Models.Task>.Update.Set(t => t.IsCompleted, true)
                );

                if (updateResult.ModifiedCount > 0)
                {
                    return RedirectToPage(new { IsAssignedToTab = true });
                }
                else
                {
                    ErrorMessage = "Error updating task status.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error updating task status: " + ex.Message;
            }

            return RedirectToPage(new { ErrorMessage });
        }

        public async Task<IActionResult> OnPostLogout()
        {
            HttpContext.Session.Remove("auth_token");
            return RedirectToPage("/Login");
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
