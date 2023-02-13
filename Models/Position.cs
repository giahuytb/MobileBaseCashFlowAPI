using System;
using System.Collections.Generic;

namespace MobieBaseCashFlowAPI.Models
{
    public partial class Position
    {
        public string PositionId { get; set; } = null!;
        public int Value { get; set; }
        public string? TileId { get; set; }

        public virtual Tile? Tile { get; set; }
    }
}
