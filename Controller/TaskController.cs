using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using TaskManagerApp.Data;
using TaskManagerApp.Models;  // Ensure this is included
using System.Threading.Tasks;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public TaskController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _dbContext.Tasks.Find(task => true).ToListAsync();
            return Ok(tasks);
        }

        // Get task by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            var task = await _dbContext.Tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        // Create new task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskManagerApp.Models.Task task)  // Fully qualify here
        {
            await _dbContext.Tasks.InsertOneAsync(task);
            return Ok(task);
        }

        // Update task
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] TaskManagerApp.Models.Task task)  // Fully qualify here
        {
            var existingTask = await _dbContext.Tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (existingTask == null)
            {
                return NotFound();
            }

            task.Id = id;
            await _dbContext.Tasks.ReplaceOneAsync(t => t.Id == id, task);
            return Ok(task);
        }

        // Delete task
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            var task = await _dbContext.Tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (task == null)
            {
                return NotFound();
            }

            await _dbContext.Tasks.DeleteOneAsync(t => t.Id == id);
            return Ok();
        }
    }
}
