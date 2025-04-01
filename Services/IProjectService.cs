using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(int id, Project project);
        Task<bool> DeleteProjectAsync(int id);
    }
}
