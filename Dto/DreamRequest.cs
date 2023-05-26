using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.Dto
{
    public class DreamRequest
    {
        [Required(ErrorMessage = "Please enter dream name")]
        [MaxLength(50, ErrorMessage = "Dream name is too long (max is 50)")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter this dream's cost")]
        [Range(0, int.MaxValue, ErrorMessage = "Cost must be mumber and bigger than 0")]
        public double Cost { get; set; } = 0;
        public int Game_mod_id { get; set; } = int.MaxValue;
    }
}
