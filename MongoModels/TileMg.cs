using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MobieBasedCashFlowAPI.MongoModels
{
    public class TileMg
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Tile_type { get; set; } = string.Empty;
        public List<int> Positions { get; set; } = new List<int>();
        public string Race_type { get; set; } = string.Empty;
    }
}
