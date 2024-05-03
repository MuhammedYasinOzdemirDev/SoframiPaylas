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
            Event eventt = new Event
            {
                HostID = eventDto.HostID,
                Title = eventDto.Title,
                Description = eventDto.Description,
                Location = eventDto.Location,
                Date = eventDto.Date,
                Time = eventDto.Time,
                ParticipantIDs = eventDto.ParticipantIDs,
                MaxParticipants = eventDto.MaxParticipants,
                Images = eventDto.Images,
                EventStatus = eventDto.EventStatus
            };
            return await _eventRepository.CreateEventAsync(eventt);
        }

        public async Task DeleteEventAsync(string id)
        {
            await _eventRepository.DeleteEventAsync(id);
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetEventAllAsync();
            return events.Select(e => new EventDto
            {
                HostID = e.HostID,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                Date = e.Date,
                Time = e.Time,
                ParticipantIDs = e.ParticipantIDs,
                MaxParticipants = e.MaxParticipants,
                Images = e.Images,
                EventStatus = e.EventStatus
            });
        }

        public async Task<EventDto> GetEventByIdAsync(string id)
        {
            var e = await _eventRepository.GetEventByIdAsync(id);
            if (e == null)
            {
                throw new KeyNotFoundException($"No event found with ID {id}");
            }
            return new EventDto
            {
                HostID = e.HostID,
                Title = e.Title,
                Description = e.Description,
                Location = e.Location,
                Date = e.Date,
                Time = e.Time,
                ParticipantIDs = e.ParticipantIDs,
                MaxParticipants = e.MaxParticipants,
                Images = e.Images,
                EventStatus = e.EventStatus
            };
        }

        public async Task UpdateEventAsync(string id, UpdateEventDto eventDto)
        {
            var eventItem = new Event
            {
                HostID = eventDto.HostID,
                Title = eventDto.Title,
                Description = eventDto.Description,
                Location = eventDto.Location,
                Date = eventDto.Date,
                Time = eventDto.Time,
                ParticipantIDs = eventDto.ParticipantIDs,
                MaxParticipants = eventDto.MaxParticipants,
                Images = eventDto.Images,
                EventStatus = eventDto.EventStatus
            };
            await _eventRepository.UpdateEventAsync(id, eventItem);
        }
    }
}