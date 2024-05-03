using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly FirebaseService _service;
        public EventRepository(FirebaseService firebaseService)
        {
            _service = firebaseService;
        }

        public async Task<string> CreateEventAsync(Event Event)
        {
            string eventId = Guid.NewGuid().ToString();
            await _service.GetDb().Collection("Events").Document(eventId).SetAsync(Event);
            return eventId;
        }
    }
}