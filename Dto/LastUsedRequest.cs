using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class LastUsedRequest
    {
        [Required(ErrorMessage = "Please enter asset id")]
        [Range(1, int.MaxValue, ErrorMessage = "Asset id must be a mumber")]
        public int AssetId { get; set; }

        public string LastJobSelected { get; set; } = string.Empty;
    }
}
