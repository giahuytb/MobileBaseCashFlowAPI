using MobileBasedCashFlowAPI.Dto;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface ISendMailRepository
    {
        Task<bool> SendMail(MailContent oMailContent);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
