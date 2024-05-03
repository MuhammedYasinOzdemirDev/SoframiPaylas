using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs.Event;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<string> CreateEventAsync(CreateEventDto eventDto)
        {
            var eventItem = _mapper.Map<Event>(eventDto);
            return await _eventRepository.CreateEventAsync(eventItem);
        }

        public async Task DeleteEventAsync(string id)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                throw new Exception("Event not found.");
            }

            await _eventRepository.DeleteEventAsync(id);
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetEventAllAsync();
            return events.Select(e => _mapper.Map<EventDto>(e));
        }

        public async Task<EventDto> GetEventByIdAsync(string id)
        {
            var e = await _eventRepository.GetEventByIdAsync(id);
            if (e == null)
            {
                throw new KeyNotFoundException($"No event found with ID {id}");
            }
            return _mapper.Map<EventDto>(e);
        }

        public async Task UpdateEventAsync(string id, UpdateEventDto eventDto)
        {
            var eventItem = await _eventRepository.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                throw new Exception("Event not found.");
            }
            _mapper.Map(eventDto, eventItem);
            await _eventRepository.UpdateEventAsync(id, eventItem);
        }
    }
}