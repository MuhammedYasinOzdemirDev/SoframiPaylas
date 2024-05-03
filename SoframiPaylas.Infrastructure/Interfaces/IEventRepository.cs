using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface IEventRepository
    {
        Task<string> CreateEventAsync(Event Event);
    }
}