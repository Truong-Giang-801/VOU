using MongoDB.Driver;

namespace Vou.Services.VoucherAPI.Data
{
    public class MongoDbService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDbService(IConfiguration configuration,IMongoDatabase mongoDatabase) 
        { 
            _configuration = configuration;
            _mongoDatabase = mongoDatabase;
            var connectionString = _configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }
        public IMongoDatabase? Database => _mongoDatabase;
    }
}
