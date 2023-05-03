using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class ParticipantRequest
    {
        [Required(ErrorMessage = "You must input userId")]
        [Range(100000, int.MaxValue, ErrorMessage = "User id must be integer value and > 1000000")]
        public int UserId { get; set; } = int.MaxValue;

        [Required(ErrorMessage = "You must input matchId")]
        public string MatchId { get; set; } = string.Empty;
    }
}
