using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameServer
    {
        public GameServer()
        {
            Games = new HashSet<Game>();
            UserAccounts = new HashSet<UserAccount>();
        }

        public int GameServerId { get; set; }
        public string GameVersion { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
