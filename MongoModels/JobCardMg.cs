//using MobieBaseCashFlowAPI.SubModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobieBaseCashFlowAPI.MongoModels
{
    public class JobCardMg
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Child_cost { get; set; } = 0;
        public  List<GameAccountMg> Game_accounts { get; set; } = new List<GameAccountMg>();

    }
}
