using MarketPlace.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
namespace MarketPlace.Web.Services
{

    public class EmailSender : IEmailSender, Microsoft.AspNetCore.Identity.IEmailSender<ApplicationUser>
    {
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            return Task.CompletedTask;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For now, do nothing
            return Task.CompletedTask;
        }

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            return Task.CompletedTask;
        }

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            return Task.CompletedTask;
        }
    }
}