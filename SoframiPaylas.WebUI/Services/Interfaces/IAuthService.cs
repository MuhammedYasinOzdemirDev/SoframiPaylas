using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> RegisterAsync(RegisterViewModel model);
    }
}