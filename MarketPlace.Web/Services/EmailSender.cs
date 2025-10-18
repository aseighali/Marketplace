using MarketPlace.Application.Interfaces;

namespace MarketPlace.Web.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // TODO: Implement actual email sending (SMTP, SendGrid, etc.)
            return Task.CompletedTask;
        }
    }
}