using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.MongoDTO
{
    public class EventCardRequest
    {
        [Required]
        public string Event_name { get; set; } = string.Empty;
        [Required]
        public string Image_url { get; set; } = string.Empty;
        public string Account_Name { get; set; } = string.Empty;
        public double Cost { get; set; } = double.MaxValue;
        public double Down_pay { get; set; } = double.MaxValue;
        public double Dept { get; set; } = double.MaxValue;
        public double Cash_flow { get; set; } = double.MaxValue;
        public string Trading_range { get; set; } = string.Empty;
        [Required]
        public string Event_description { get; set; } = string.Empty;
        [Required]
        public int Event_type_id { get; set; } = int.MaxValue;
        [Required]
        public int Action { get; set; } = int.MaxValue;
    }
}
