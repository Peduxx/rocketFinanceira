using System.Net;
using System.Net.Mail;

namespace BillingService.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _senderEmail = "claudivan15014_matos@hotmail.com";
        private readonly string _senderPassword = "FPSforever123";

        public async Task SendEmail(string recipientEmail, string subject, string body)
        {
            using var message = new MailMessage(_senderEmail, recipientEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var smtpClient = new SmtpClient("smtp.mail.outlook.com");
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(message);
        }
    }
}
