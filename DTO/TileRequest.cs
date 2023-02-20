namespace MobileBasedCashFlowAPI.DTO
{
    public class TileRequest
    {
        public bool IsRatRace { get; set; }
        public string GameEventName { get; set; } = null!;
        public string DreamName { get; set; } = null!;
        public string TileTypeName { get; set; } = null!;
        public string BoardId { get; set; } = null!;
    }
}
