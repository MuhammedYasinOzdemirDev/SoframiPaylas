using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly FirebaseService _service;
        public PostRepository(FirebaseService service)
        {
            _service = service;
        }
    }
}