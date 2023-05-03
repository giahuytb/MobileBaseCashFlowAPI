using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class AssetTypeRequest
    {
        [Required(ErrorMessage = "Please enter asset type name")]
        [MaxLength(30, ErrorMessage = "Do not enter more than 30 characters")]
        public string AssetTypeName { get; set; } = string.Empty;
    }
}
