using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BillingService.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            // Configurações do servidor SMTP
            _smtpClient = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                Credentials = new NetworkCredential("seuEmail", "suaSenha"),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MailMessage
            {
                From = new MailAddress("seuEmail"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await _smtpClient.SendMailAsync(message);
        }
    }
}
