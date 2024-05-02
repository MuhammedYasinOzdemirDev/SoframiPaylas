using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI.Controllers
{
    public class EventController
    {
        private readonly IEventService _service;
        public EventController(IEventService service)
        {
            _service = service;
        }

    }
}