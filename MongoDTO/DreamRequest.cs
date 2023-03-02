namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class DreamRequest
    {
        public string? id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Cost { get; set; } = double.MaxValue;
    }
}
