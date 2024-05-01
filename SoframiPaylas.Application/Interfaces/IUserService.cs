using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser();
    }
}