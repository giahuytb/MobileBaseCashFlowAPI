namespace MobileBasedCashFlowAPI.DTO
{
    public class TileRequest
    {
        public bool IsRatRace { get; set; }
        public string EventId { get; set; } = null!;
        public string DreamId { get; set; } = null!;
        public string TileTypeId { get; set; } = null!;
        public string BoardId { get; set; } = null!;
    }
}
