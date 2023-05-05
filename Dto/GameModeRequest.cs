using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class GameModeRequest
    {
        [Required(ErrorMessage = "Please enter game mode name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string ModeName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter image url for this game mode")]
        [MaxLength(200, ErrorMessage = "Image url is too long (mas is 200)")]
        public string ImageUrl { get; set; } = null!;

        [Required(ErrorMessage = "Please enter description")]
        [MaxLength(500, ErrorMessage = "Description is too long (max is 500)")]
        public string Description { get; set; } = null!;

    }
}
