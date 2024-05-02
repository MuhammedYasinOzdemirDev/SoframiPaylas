using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}