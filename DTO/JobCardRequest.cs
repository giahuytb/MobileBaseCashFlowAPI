namespace MobileBasedCashFlowAPI.DTO
{
    public class JobCardRequest
    {
        public string JobName { get; set; } = null!;
        public string JobImageUrl { get; set; } = null!;
        public double ChildrenCost { get; set; }
    }
}
