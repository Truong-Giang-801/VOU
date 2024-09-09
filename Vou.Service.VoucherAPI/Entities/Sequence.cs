using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
namespace Vou.Service.VoucherAPI.Entities
{
    public class Sequence
    {
        [BsonId]
        public string Id { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}
