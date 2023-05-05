
using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class AssetRequest
    {
        [Required(ErrorMessage = "Please enter asset name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 30 characters")]
        public string AssetName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter image url for this asset")]
        [MaxLength(200, ErrorMessage = "Image url is too long (mas is 200)")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Please enter asset price")]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be a mumber and bigger than 0")]
        public double AssetPrice { get; set; }

        [Required(ErrorMessage = "Please enter description")]
        [MaxLength(500, ErrorMessage = "Description is too long (max is 500)")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Please choice it in shop or not")]
        public bool IsInShop { get; set; }

        [Required(ErrorMessage = "Please enter asset type")]
        [Range(1, int.MaxValue, ErrorMessage = "Asset type must be number and bigger than 0")]
        public byte AssetType { get; set; }
    }
}
