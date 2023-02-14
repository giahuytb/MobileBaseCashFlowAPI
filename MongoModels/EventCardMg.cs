using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MobieBasedCashFlowAPI.MongoModels
{
    public class EventCardMg
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Event_name { get; set; } = string.Empty;
        public double Cost { get; set; } = double.MaxValue;
        public double Down_pay { get; set; } = double.MaxValue;
        public double Dept { get; set; } = double.MaxValue;
        public double Cash_flow { get; set; } = double.MaxValue;
        public string Trading_range { get; set; } = string.Empty;
        public string Event_description { get; set; } = string.Empty;
        public int Event_type_id { get; set; } = int.MaxValue;
        public int Action { get; set; } = int.MaxValue;

    }
}
