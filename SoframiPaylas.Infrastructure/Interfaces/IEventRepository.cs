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
        Task<List<Event>> GetEventAllAsync();
        Task<Event> GetEventByIdAsync(string id);
        Task UpdateEventAsync(string id, Event Event);
        Task DeleteEventAsync(string id);

    }
}