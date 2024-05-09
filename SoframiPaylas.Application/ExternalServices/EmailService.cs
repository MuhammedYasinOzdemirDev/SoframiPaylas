using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SoframiPaylas.Application.ExternalServices.Interfaces;



namespace SoframiPaylas.Application.ExternalServices
{
    public class EmailService : IEmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _fromAddress;
        private readonly string _username;
        private readonly string _passaword;

        public EmailService(string smtpHost, int smtpPort, string fromAddress, string username, string password)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _fromAddress = fromAddress;
            _username = username;
            _passaword = password;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {

            using (var client = new SmtpClient(_smtpHost, _smtpPort))
            {
                // SSL'yi etkinleştir
                client.EnableSsl = true;

                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_fromAddress, _passaword);
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromAddress),
                    Subject = subject,
                    Body = htmlContent,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    // Hata işleme
                    throw new InvalidOperationException($"Mail gönderme işlemi sırasında bir hata oluştu: {ex.Message}", ex);
                }
            }
        }

    }

}