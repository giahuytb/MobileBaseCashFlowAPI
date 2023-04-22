using MobileBasedCashFlowAPI.Common;
using System.ComponentModel.DataAnnotations;

namespace MobileBasedCashFlowAPI.DTO
{
    public class EditProfileRequest
    {
        [MinLength(5, ErrorMessage = "Min length is 5"), MaxLength(30, ErrorMessage = "Max length is 30")]
        public string NickName { get; set; } = string.Empty;
        [RegularExpression("^male$|^female$")]
        public string Gender { get; set; } = string.Empty;
        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Please enter the correct phone number")]
        public string Phone { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "PLease Enter the correct Email Address")]
        public string Email { get; set; } = string.Empty;
        [MaxLength(200, ErrorMessage = "Image url max length is 200 character")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
