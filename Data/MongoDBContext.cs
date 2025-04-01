using MongoDB.Driver;
using TaskManagerApp.Models;
using TaskModel = TaskManagerApp.Models.Task;  // Alias for Task

namespace TaskManagerApp.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase(configuration["MongoDB:Database"]);
        }

        // Use the alias TaskModel for your Task class
        public IMongoCollection<TaskModel> Tasks => _database.GetCollection<TaskModel>("Tasks");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}
