using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MobileBasedCashFlowAPI.MongoModels
{
    public class FinancialReport
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public int Children_amount { get; set; } = 0;
        public string User_id { get; set; } = string.Empty;
        public string Job_card_id { get; set; } = string.Empty;
        public double Income_per_month { get; set; } = 0;
        public double Expense_per_month { get; set; } = 0;
        public double Passive_income { get; set; } = 0;
        public List<GameAccount> Game_accounts { get; set; } = new List<GameAccount>();
    }
}
