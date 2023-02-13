using MobileBaseCashFlowGameAPI.ViewModels;

namespace MobileBaseCashFlowGameAPI.IServices
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request);
        Task<string> Register(RegisterRequest request);
        Task<string> VerifyEmail(string token);
        Task<bool> ForgotPassword(string email);
        Task<bool> ResetPassword(ResetPasswordRequest request);
    }
}
