using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MobileBasedCashFlowAPI.Dto
{
    public class EditProfileRequest
    {
        [MinLength(2, ErrorMessage = "Min length is 5"), MaxLength(12, ErrorMessage = "Max length is 12")]
        [AllowNull]
        public string? NickName { get; set; }

        [RegularExpression("^male$|^female$")]
        [AllowNull]
        public string? Gender { get; set; }

        [RegularExpression(@"(84|0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Please enter the correct phone number")]
        [AllowNull]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "PLease Enter the correct Email Address")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Image url max length is 200 character")]
        [AllowNull]
        public string? ImageUrl { get; set; }
    }
}
