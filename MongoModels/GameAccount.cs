using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace MobileBasedCashFlowAPI.MongoModels
{
    public class GameAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public int Game_account_type_id { get; set; }
        public string Game_account_name { get; set; } = null!;
        public DateTime Create_at { get; set; }
    }
}
