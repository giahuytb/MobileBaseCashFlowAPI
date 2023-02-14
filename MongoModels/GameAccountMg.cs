using System.Collections;

namespace MobieBasedCashFlowAPI.MongoModels
{
    public class GameAccountMg
    {
        public int GameAccount_type { get; set; } = 0;
        public string GameAccount_name { get; set; } = string.Empty;
        public double GameAccount_cost { get; set; } = 0;
    }
}
