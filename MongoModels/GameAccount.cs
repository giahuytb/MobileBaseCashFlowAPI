using System.Collections;

namespace MobileBasedCashFlowAPI.MongoModels
{
    public class GameAccount
    {
        public int GameAccount_type { get; set; } = 0;
        public string GameAccount_name { get; set; } = string.Empty;
        public double GameAccount_cost { get; set; } = 0;
    }
}
