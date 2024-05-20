using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> RegisterAsync(RegisterViewModel model);
        Task<HttpResponseMessage> LoginAsync(LoginViewModel model);
        Task<FirebaseUser> VerifyUser(string IdToken);
        Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordRequest model);
    }
}