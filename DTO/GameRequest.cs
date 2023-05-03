using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class GameRequest
    {
        [Required(ErrorMessage = "Please enter game version")]
        [MaxLength(30, ErrorMessage = "Do not enter more than 30 characters")]
        public string GameVersion { get; set; } = string.Empty;
    }
}
