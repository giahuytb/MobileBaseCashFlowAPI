namespace MobileBasedCashFlowAPI.DTO
{
    public class DreamRequest
    {
        public string DreamName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Cost { get; set; }
        public string DreamImageUrl { get; set; } = null!;
    }
}
