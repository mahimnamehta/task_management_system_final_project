using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerApp.Models;
using TaskManagerApp.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using TaskManagerApp.Services;

namespace TaskManagerApp.Pages
{
    public class ProjectService : IProjectService
    {
        private readonly MongoDbContext _dbContext;

        public ProjectService(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all projects
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _dbContext.Projects.Find(project => true).ToListAsync();
        }

        // Get a project by ID
        public async Task<Project> GetProjectByIdAsync(int id)
        {
            // Convert the integer id to ObjectId
            ObjectId objectId = ObjectId.Parse(id.ToString());

            // Find project by ObjectId
            return await _dbContext.Projects
                .Find(project => project.Id == objectId)
                .FirstOrDefaultAsync();
        }

        // Create a new project
        public async Task<Project> CreateProjectAsync(Project project)
        {
            await _dbContext.Projects.InsertOneAsync(project);
            return project;
        }

        // Update an existing project
        public async Task<Project> UpdateProjectAsync(int id, Project project)
        {
            // Convert the integer id to ObjectId
            ObjectId objectId = ObjectId.Parse(id.ToString());

            // Find the project by ObjectId
            var existingProject = await _dbContext.Projects.Find(p => p.Id == objectId).FirstOrDefaultAsync();
            if (existingProject == null)
            {
                return null;
            }

            project.Id = objectId; // Ensure the project ID remains unchanged
            await _dbContext.Projects.ReplaceOneAsync(p => p.Id == objectId, project);
            return project;
        }


        // Delete a project
        public async Task<bool> DeleteProjectAsync(int id)
        {
            // Convert the integer id to ObjectId
            ObjectId objectId = ObjectId.Parse(id.ToString());

            // Delete the project by ObjectId
            var result = await _dbContext.Projects.DeleteOneAsync(p => p.Id == objectId);
            return result.DeletedCount > 0;
        }
    }
}
