using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class PointOfInteraction
    {
        public int PoiId { get; set; }
        public string PoiName { get; set; } = null!;
        public string? PoiDescription { get; set; }
        public string? PoiVideoUrl { get; set; }
        public DateTime CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public bool Status { get; set; }
        public int? GameId { get; set; }

        public virtual Game? Game { get; set; }
    }
}
