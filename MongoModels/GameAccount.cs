using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace MobileBasedCashFlowAPI.MongoModels
{
    public class GameAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string Game_account_type { get; set; } = null!;
        public string Game_account_name { get; set; } = null!;
        public bool Status { get; set; }
    }
}
