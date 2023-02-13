using MobileBaseCashFlowGameAPI.ViewModels;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MobileBaseCashFlowGameAPI.IServices;
using MobieBaseCashFlowAPI.Settings;

namespace MobileBaseCashFlowGameAPI.Services
{

    public class SendMailService : ISendMailService
    {
        private readonly MailSettings _mailSettings;

        public SendMailService(IOptions<MailSettings> mailSettings)
        {   
            _mailSettings = mailSettings.Value;
        }   
        public async Task<bool> SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.UserName, _mailSettings.From);
            email.From.Add( new MailboxAddress(_mailSettings.UserName, _mailSettings.From));

            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient(); 
            try 
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.From, _mailSettings.Password);
                await smtp.SendAsync(email);              
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            smtp.Disconnect(true);
            return true;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }
    }
}
