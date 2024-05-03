using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Event;

namespace SoframiPaylas.Application.Interfaces
{
    public interface IEventService
    {
        Task<string> CreateEventAsync(CreateEventDto eventDto);
    }
}