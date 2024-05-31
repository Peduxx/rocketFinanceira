namespace BillingService.Infrastructure.Email
{
    public interface IEmailService
    {
        Task SendEmail(string recipientEmail, string subject, string body);
    }
}
