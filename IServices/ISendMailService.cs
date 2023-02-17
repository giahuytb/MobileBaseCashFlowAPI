using MobileBasedCashFlowAPI.DTO;

namespace MobileBasedCashFlowAPI.IServices
{
    public interface ISendMailService
    {
        Task<bool> SendMail(MailContent oMailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
