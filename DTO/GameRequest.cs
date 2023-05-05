using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class GameRequest
    {
        [Required(ErrorMessage = "Please enter game version")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 20 characters")]
        public string GameVersion { get; set; } = string.Empty;
    }
}
