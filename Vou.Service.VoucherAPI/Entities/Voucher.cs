using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vou.Services.VoucherAPI.Models
{
    public class Voucher
    {
        [BsonId]
        [BsonElement("_id"),BsonRepresentation(BsonType.Int32)]
        public int Id { get; set; }
        [BsonElement("_code"), BsonRepresentation(BsonType.String)]
        public string Code { get; set; } = string.Empty;
        [BsonElement("_qrCode"), BsonRepresentation(BsonType.String)]
        public string QRCode { get; set; } = string.Empty;
        [BsonElement("_img"), BsonRepresentation(BsonType.String)]
        public string Img { get; set; } = string.Empty;
        [BsonElement("_description"), BsonRepresentation(BsonType.String)]

        public string Description { get; set; } = string.Empty;
        [BsonElement("_value"), BsonRepresentation(BsonType.Int32)]
        public int Value { get; set; }
        [BsonElement("_state"), BsonRepresentation(BsonType.Boolean)]

        public bool State { get; set; } = false;
        [BsonElement("_expire_date"), BsonRepresentation(BsonType.DateTime)]
        public DateTime ExpireDate { get; set; }



    }
}
