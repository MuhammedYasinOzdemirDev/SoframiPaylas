using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface IParticipantApiService
    {
        Task<HttpResponseMessage> Join(string userId, string postId);
    }
}