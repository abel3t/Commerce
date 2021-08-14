using MongoDB.Driver;

namespace Commerce.Core.Repository
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }

    public interface IMongoDbContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoDbContext : IMongoDbContext
    {
        public MongoDbContext(IMongoDatabase database)
        {
            Database = database;
        }

        public MongoDbContext(IMongoClient client, MongoDbOptions options)
        {
            Database = client.GetDatabase(options.Database);
        }

        public MongoDbContext(MongoDbOptions options)
        {
            Database = new MongoClient(options.ConnectionString).GetDatabase(options.Database);
        }

        public IMongoDatabase Database { get; }
    }
}