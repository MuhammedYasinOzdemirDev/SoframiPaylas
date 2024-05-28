

namespace SoframiPaylas.Application.ExternalServices.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlContent);
        Task SendEmailsAsync(IEnumerable<string> toList, string subject, string htmlContent);
    }

}