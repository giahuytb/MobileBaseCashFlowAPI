using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.DTO
{
    public class GameRequest
    {
        [Required(ErrorMessage = "Please enter room name")]
        [MaxLength(30, ErrorMessage = "Do not enter more than 30 characters")]
        public string RoomName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter room number")]
        [MaxLength(30, ErrorMessage = "Do not enter more than 30 characters")]
        public string RoomNumber { get; set; } = null!;
    }
}
