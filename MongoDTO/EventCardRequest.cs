using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class EventCardRequest
    {
        [Required(ErrorMessage = "Please enter your event card name")]
        public string Event_name { get; set; } = string.Empty;
        public string Image_url { get; set; } = string.Empty;

        public string Account_Name { get; set; } = string.Empty;
        [Range(0, double.MaxValue, ErrorMessage = "Please enter number greater than or equal to 0")]
        public double Cost { get; set; } = double.MaxValue;
        [Range(0, double.MaxValue, ErrorMessage = "Please enter number greater than or equal to 0")]
        public double Down_pay { get; set; } = double.MaxValue;
        [Range(0, double.MaxValue, ErrorMessage = "Please enter number greater than or equal to 0")]
        public double Dept { get; set; } = double.MaxValue;
        [Range(0, double.MaxValue, ErrorMessage = "Please enter number greater than or equal to 0")]
        public double Cash_flow { get; set; } = double.MaxValue;
        [MinLength(5), MaxLength(50)]
        public string Trading_range { get; set; } = string.Empty;
        [Required]
        [MinLength(5), MaxLength(50)]
        public string Event_description { get; set; } = string.Empty;
        [Required]
        [Range(1, 6, ErrorMessage = "Event type must between 1 to 6")]
        public int Event_type_id { get; set; } = int.MaxValue;
        [Required]
        [Range(1, 7, ErrorMessage = "Acction must between 1 to 7")]
        public int Action { get; set; } = int.MaxValue;
    }
}
