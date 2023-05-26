using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class JobCardRequest
    {
        [Required(ErrorMessage = "Please enter job card name")]
        [MaxLength(20, ErrorMessage = "")]
        public string Job_card_name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter children cost")]
        [Range(0, int.MaxValue, ErrorMessage = "Children cost must be mumber and bigger than 0")]
        public int Children_cost { get; set; } = 0;

        [Required(ErrorMessage = "Please enter image url for this jobcard")]
        [MaxLength(200, ErrorMessage = "Image url is too long (max is 200)")]
        public string Image_url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter game account")]
        public List<GameAccountRequest> Game_accounts { get; set; } = new List<GameAccountRequest>();
    }
}
