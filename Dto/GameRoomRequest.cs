using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class GameRoomRequest
    {
        [Required(ErrorMessage = "Please enter room name")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 30 characters")]
        public string RoomName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter room number")]
        [MaxLength(20, ErrorMessage = "Do not enter more than 30 characters")]
        public string RoomNumber { get; set; } = null!;
    }
}
