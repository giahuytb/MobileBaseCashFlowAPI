using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobileBasedCashFlowAPI.MongoModels
{
    public class Dream
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Cost { get; set; } = double.MaxValue;
        public bool Status { get; set; }
        public int Game_mod_id { get; set; } = int.MaxValue;

    }
}
