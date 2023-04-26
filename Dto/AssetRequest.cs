namespace MobileBasedCashFlowAPI.DTO
{
    public class AssetRequest
    {
        public string AssetName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public double AssetPrice { get; set; }
        public string Description { get; set; } = null!;
        public bool IsInShop { get; set; }
        public byte AssetType { get; set; }
    }
}
