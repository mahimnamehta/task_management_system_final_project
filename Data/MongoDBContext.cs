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
            var connectionString = configuration.GetConnectionString("MongoDB");
            var databaseName = configuration["MongoDB:Database"];

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("MongoDB connection string is missing in configuration.");
            }

            if (string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("MongoDB database name is missing in configuration.");
            }

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        // Use the alias TaskModel for your Task class
        public IMongoCollection<TaskModel> Tasks => _database.GetCollection<TaskModel>("Tasks");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Project> Projects => _database.GetCollection<Project>("Projects");
    }
}
