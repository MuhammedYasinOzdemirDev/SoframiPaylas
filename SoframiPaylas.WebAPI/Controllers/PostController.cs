using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.Application.DTOs.Post;
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
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostByIdAsync(string postId)
        {
            var post = await _postservice.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound("Post not found");
            return Ok(post);
        }
        [HttpPost("post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {
            var postId = await _postservice.CreatePostAsync(postDto);
            if (postId == null)
                return BadRequest();
            return Ok(postId);
        }
        [HttpPut("post")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto postDto, string postId)
        {
            await _postservice.UpdatePostAsync(postDto, postId);
            return Ok();
        }
        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            await _postservice.DeletePostAsync(postId);
            return Ok();
        }

    }
}