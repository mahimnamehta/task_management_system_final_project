using MongoDB.Driver;
namespace task_management_app.Data
{
    public class MongoDBContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings");
            var connectionString = mongoSettings["ConnectionString"];
            var databaseName = mongoSettings["DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>(nameof(Product));
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>(nameof(Category));
        public IMongoCollection<User> Users => _database.GetCollection<User>(nameof(User));
    }
}
