namespace MobileBasedCashFlowAPI.DTO
{
    public class EditProfileRequest
    {
        public string NickName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
