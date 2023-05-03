using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameRoom
    {
        public GameRoom()
        {
            GameMatches = new HashSet<GameMatch>();
            GameMods = new HashSet<GameMod>();
        }

        public int GameRoomId { get; set; }
        public string RoomNumber { get; set; } = null!;
        public string RoomName { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public bool Status { get; set; }
        public int? GameId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual ICollection<GameMatch> GameMatches { get; set; }
        public virtual ICollection<GameMod> GameMods { get; set; }
    }
}
