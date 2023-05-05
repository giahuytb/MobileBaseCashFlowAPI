using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class POIRequest
    {
        [Required(ErrorMessage = "Please enter POI name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string PoiName { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Description is too long (max is 500)")]
        public string? PoiDescription { get; set; }

        [MaxLength(200, ErrorMessage = "Video url is too long (mas is 200)")]
        public string? PoiVideoUrl { get; set; }

        [Required(ErrorMessage = "Please enter game id")]
        public int? GameId { get; set; }
    }
}
