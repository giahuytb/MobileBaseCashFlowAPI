using System;
using System.Collections.Generic;

namespace MobieBaseCashFlowAPI.Models
{
    public partial class Game
    {
        public Game()
        {
            Boards = new HashSet<Board>();
            EventCards = new HashSet<EventCard>();
            GameMatches = new HashSet<GameMatch>();
            Leaderboards = new HashSet<Leaderboard>();
            UserAccounts = new HashSet<UserAccount>();
        }

        public string GameId { get; set; } = null!;
        public string GameVersion { get; set; } = null!;
        public string BackgroundImageUrl { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<Board> Boards { get; set; }
        public virtual ICollection<EventCard> EventCards { get; set; }
        public virtual ICollection<GameMatch> GameMatches { get; set; }
        public virtual ICollection<Leaderboard> Leaderboards { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
