using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postrepository;

        public PostService(IPostRepository postRepository)
        {
            _postrepository = postRepository;
        }
    }
}