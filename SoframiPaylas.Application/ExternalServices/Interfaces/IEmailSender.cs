using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Application.ExternalServices.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string htmlContent);
    }

}