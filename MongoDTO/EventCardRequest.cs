using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class EventCardRequest
    {
        [Required(ErrorMessage = "Please enter your event card name")]
        public string Event_name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please choice image for this event card")]
        public string Image_url { get; set; } = string.Empty;

        public string Account_Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Please enter number bigger than or equal to 0")]
        public double Cost { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Please enter number bigger than or equal to 0")]
        public double Down_pay { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Please enter number bigger than or equal to 0")]
        public double Dept { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Please enter number bigger than or equal to 0")]
        public double Cash_flow { get; set; } = 0;

        [MaxLength(50, ErrorMessage = "Tranding range is too long (max is 50)")]
        public string Trading_range { get; set; } = string.Empty;

        [Required]
        [MaxLength(500, ErrorMessage = "Description is too long (max is 500)")]
        public string Event_description { get; set; } = string.Empty;

        [Required]
        [Range(1, 6, ErrorMessage = "Event type must between 1 to 6")]
        public int Event_type_id { get; set; } = int.MaxValue;

        [Required(ErrorMessage = "Please choice your game mode")]
        public int Game_mode_id { get; set; } = int.MaxValue;

        [Required]
        [Range(1, 7, ErrorMessage = "Acction must between 1 to 7")]
        public int Action { get; set; } = int.MaxValue;
    }
}
