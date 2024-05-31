﻿namespace BillingService.Infrastructure.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
