using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Game
    {
        public Game()
        {
            GameMatches = new HashSet<GameMatch>();
            GameModes = new HashSet<GameMode>();
        }

        public int GameId { get; set; }
        public string RoomNumber { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public int? GameServerId { get; set; }

        public virtual GameServer? GameServer { get; set; }
        public virtual ICollection<GameMatch> GameMatches { get; set; }
        public virtual ICollection<GameMode> GameModes { get; set; }
    }
}
