using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobieBasedCashFlowAPI.MongoModels
{
    public class DreamMg
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Cost { get; set; } = double.MaxValue;
    }
}
