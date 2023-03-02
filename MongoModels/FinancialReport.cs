using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MobileBasedCashFlowAPI.MongoDTO;

namespace MobileBasedCashFlowAPI.MongoModels
{
    public class FinancialReport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string Job_card_id { get; set; } = string.Empty;
        public int Children_amount { get; set; } = 0;
        public string User_id { get; set; } = string.Empty;
        public DateTime Create_at { get; set; }
        public List<GameAccountRequest> Game_accounts { get; set; } = new List<GameAccountRequest>();
    }
}
