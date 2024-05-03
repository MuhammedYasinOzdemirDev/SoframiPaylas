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
        [HttpPost("event")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventDto)
        {
            var eventId = await _service.CreateEventAsync(eventDto);
            if (string.IsNullOrEmpty(eventId))
                return BadRequest();
            return Ok(eventId);
        }
    }
}