using System;
using System.Collections.Generic;

namespace MobieBasedCashFlowAPI.Models
{
    public partial class Tile
    {
        public Tile()
        {
            Positions = new HashSet<Position>();
        }

        public string TileId { get; set; } = null!;
        public bool IsRatRace { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public string? EventId { get; set; }
        public string? DreamId { get; set; }
        public string? TileTypeId { get; set; }
        public string? BoardId { get; set; }

        public virtual Board? Board { get; set; }
        public virtual Dream? Dream { get; set; }
        public virtual GameEvent? Event { get; set; }
        public virtual TileType? TileType { get; set; }
        public virtual ICollection<Position> Positions { get; set; }
    }
}
