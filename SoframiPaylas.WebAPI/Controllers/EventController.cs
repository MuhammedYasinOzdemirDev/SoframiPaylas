using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoframiPaylas.Application.DTOs.Event;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : Controller
    {

        private readonly IEventService _service;
        public EventController(IEventService service)
        {
            _service = service;
        }
        [HttpGet("events")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _service.GetAllEventsAsync();
            if (events == null)
            {
                return NotFound();
            }
            return Ok(events);
        }
        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventById(string eventId)
        {
            var eventItem = await _service.GetEventByIdAsync(eventId);
            if (eventItem == null)
                return NotFound("Event not found");
            return Ok(eventItem);
        }

        [HttpPost("event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            var eventId = await _service.CreateEventAsync(eventDto);
            if (string.IsNullOrEmpty(eventId))
                return BadRequest();
            return Ok(eventId);
        }

        [HttpPut("event/{eventID}")]
        public async Task<IActionResult> UpdateEventById([FromBody] UpdateEventDto eventDto, string eventID)
        {
            await _service.UpdateEventAsync(eventID, eventDto);
            return Ok();
        }
    }
}