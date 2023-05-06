using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface IUserRepository
    {
        Task<object> Authenticate(LoginRequest request);
        Task<string> Register(RegisterRequest request);
        Task<string> VerifyEmail(string token);
        Task<bool> ForgotPassword(string userName, string email);
        Task<bool> ResetPassword(ResetPasswordRequest request);
        Task<IEnumerable> GetAsync();
        public Task<string> EditProfile(int userId, EditProfileRequest request);
        public Task<object?> ViewProfile(int userId);
        public Task<string> UpdateCoin(int userId, EditRequest request);
        public Task<object?> FindUserById(int userId);
        public Task<object?> GetUserAsset(int userId);
        public Task<string> BuyAsset(int assetId, int userId);
        public Task<string> UpdateLastUsed(int assetId, int userId);
        public Task<string> DeleteMyAsset(int assetId, int userId);
    }
}
