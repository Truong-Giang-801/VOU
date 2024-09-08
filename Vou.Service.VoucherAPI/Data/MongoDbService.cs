using MongoDB.Driver;

namespace Vou.Services.VoucherAPI.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDbService(IConfiguration configuration, IMongoClient mongoClient)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            _mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase Database => _mongoDatabase;
    }
}
