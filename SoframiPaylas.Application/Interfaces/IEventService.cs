using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Event;
using SoframiPaylas.Application.DTOs.Post;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IEventService
    {
        Task<string> CreateEventAsync(CreateEventDto eventDto);
        Task<IEnumerable<EventDto>> GetAllEventsAsync();
        Task<EventDto> GetEventByIdAsync(string id);
        Task UpdateEventAsync(string id, UpdateEventDto eventDto);
    }
}