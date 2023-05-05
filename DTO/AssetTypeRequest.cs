using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class AssetTypeRequest
    {
        [Required(ErrorMessage = "Please enter asset type name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string AssetTypeName { get; set; } = string.Empty;
    }
}
