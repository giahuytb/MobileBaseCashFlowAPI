using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface UserRepository
    {
        Task<object> Authenticate(LoginRequest request);
        Task<string> Register(RegisterRequest request);
        Task<string> VerifyEmail(string token);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<IEnumerable> GetAsync();
        public Task<string> EditProfile(int userId, EditProfileRequest request);
        public Task<object?> ViewProfile(int userId);
        public Task<string> UpdateCoin(int userId, int coin);
        public Task<object?> FindUserById(int userId);
    }
}
