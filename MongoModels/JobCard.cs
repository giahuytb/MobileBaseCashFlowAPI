//using MobileBasedCashFlowAPI.SubModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobileBasedCashFlowAPI.MongoModels
{
    public class JobCard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Children_cost { get; set; } = 0;
        public  List<GameAccount> Game_accounts { get; set; } = new List<GameAccount>();

    }
}
