using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postservice;

        public PostController(IPostService postService)
        {
            _postservice = postService;
        }
        [HttpGet("posts")]
        public async Task<IActionResult> GetAllPostAsync()
        {
            var posts = await _postservice.GetAllPostAsync();
            if (posts == null)
                return NotFound("Post not found");
            return Ok(posts);
        }


    }
}