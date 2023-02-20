
namespace MobileBasedCashFlowAPI.Models
{
    public partial class Board
    {
        public Board()
        {
            Tiles = new HashSet<Tile>();
        }

        public string BoardId { get; set; } = null!;
        public int AmountRatTile { get; set; }
        public int AmountFatTile { get; set; }
        public double DementionBoard { get; set; }
        public double RadiusRatTile { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateAt { get; set; }
        public string? UpdateBy { get; set; }
        public string? GameId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual ICollection<Tile> Tiles { get; set; }
    }
}
