using System;
using System.Collections.Generic;

namespace MobieBaseCashFlowAPI.Models
{
    public partial class GameEvent
    {
        public GameEvent()
        {
            EventCards = new HashSet<EventCard>();
            Tiles = new HashSet<Tile>();
        }

        public string EventId { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public bool IsEventTile { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<EventCard> EventCards { get; set; }
        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
