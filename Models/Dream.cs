using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class Dream
    {
        public Dream()
        {
            Tiles = new HashSet<Tile>();
        }

        public string DreamId { get; set; } = null!;
        public string DreamName { get; set; } = null!;
        public double Cost { get; set; }
        public string DreamImageUrl { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public string Description { get; set; } = null!;

        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
