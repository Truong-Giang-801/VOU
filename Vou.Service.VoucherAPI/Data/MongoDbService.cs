using MongoDB.Driver;
using Vou.Service.VoucherAPI.Entities; // Ensure this matches your actual namespace

namespace Vou.Services.VoucherAPI.Data
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Sequence> _sequenceCollection;

        public MongoDbService(IConfiguration configuration, IMongoClient mongoClient)
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            _mongoDatabase = mongoClient.GetDatabase(mongoUrl.DatabaseName);
            _sequenceCollection = _mongoDatabase.GetCollection<Sequence>("sequences"); // Ensure this collection name is correct
        }

        public async Task<int> GetNextSequenceValueAsync(string sequenceName)
        {
            var filter = Builders<Sequence>.Filter.Eq(s => s.Id, sequenceName);
            var update = Builders<Sequence>.Update.Inc(s => s.Value, 1);
            var options = new FindOneAndUpdateOptions<Sequence>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
            var result = await _sequenceCollection.FindOneAndUpdateAsync(filter, update, options);
            return result?.Value ?? 1; // Return 1 if no result is found
        }

        public IMongoDatabase Database => _mongoDatabase;
    }
}
