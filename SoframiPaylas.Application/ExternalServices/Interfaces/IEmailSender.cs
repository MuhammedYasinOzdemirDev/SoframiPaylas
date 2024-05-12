

namespace SoframiPaylas.Application.ExternalServices.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlContent);
    }

}