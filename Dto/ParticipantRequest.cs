using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.DTO
{
    public class ParticipantRequest
    {
        [Required(ErrorMessage = "You must input userId")]
        [Range(100000, int.MaxValue, ErrorMessage = "User id must be integer value and > 1000000")]
        public int UserId { get; set; } = int.MaxValue;

        [Required(ErrorMessage = "You must input matchId")]
        [Range(0, int.MaxValue, ErrorMessage = "Match id must be integer value ")]
        public int MatchId { get; set; } = int.MaxValue;
    }
}
