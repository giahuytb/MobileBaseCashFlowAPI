using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Game
    {
        public Game()
        {
            GameMods = new HashSet<GameMod>();
        }

        public int GameId { get; set; }
        public string GameName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public bool Status { get; set; }
        public int? GameServerId { get; set; }

        public virtual GameServer? GameServer { get; set; }
        public virtual ICollection<GameMod> GameMods { get; set; }
    }
}
