using MobileBasedCashFlowAPI.DTO;

namespace MobileBasedCashFlowAPI.Repository
{
    public interface SendMailRepository
    {
        Task<bool> SendMail(MailContent oMailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
