using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MobileBasedCashFlowAPI.Dto
{
    public class GameMatchRequest
    {
        [Required(ErrorMessage = "Please enter max number player of this match")]
        [Range(1, 4, ErrorMessage = "Max number of player must be from 1 - 4")]
        public int MaxNumberPlayer { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "winner id must be number")]
        [AllowNull]
        public int? WinnerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Last host id must be number")]
        [AllowNull]
        public int? LastHostId { get; set; }

        [Required(ErrorMessage = "Please enter total round of this match")]
        [Range(1, int.MaxValue, ErrorMessage = "Total round must be number and bigger than 1")]
        public int TotalRound { get; set; }

        [Required(ErrorMessage = "Please enter total round of this match")]
        [Range(0, int.MaxValue, ErrorMessage = "Game room id must be number and bigger than 0")]
        public int gameModId { get; set; }


    }
}
