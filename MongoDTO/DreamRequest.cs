using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class DreamRequest
    {
        [Required(ErrorMessage = "Please enter dream name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter this dream's cost")]
        [Range(0, int.MaxValue, ErrorMessage = "Cost must be mumber and bigger than 0")]
        public double Cost { get; set; } = 0;
    }
}
