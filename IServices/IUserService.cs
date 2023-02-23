using MobileBasedCashFlowAPI.DTO;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<string> Register(RegisterRequest request);
        Task<string> VerifyEmail(string token);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<IEnumerable> GetAsync();
        public Task<string> EditProfile(string userId, EditProfileRequest request);

    }
}
