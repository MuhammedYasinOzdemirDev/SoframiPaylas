using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoframiPaylas.Application.DTOs.Participant;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.Interfaces;

namespace SoframiPaylas.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService _postservice;
        private readonly IParticipantService _participantService;

        public PostController(IPostService postService, IParticipantService participantService)
        {
            _postservice = postService;
            _participantService = participantService;
        }
        [HttpGet("posts")]
        public async Task<IActionResult> GetAllPostAsync()
        {
            var posts = await _postservice.GetAllPostsAsync();
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
            await _postservice.UpdatePostAsync(postId, postDto);
            return Ok();
        }
        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            await _postservice.DeletePostAsync(postId);
            return Ok();
        }

        [HttpPost("{postId}/join")]
        public async Task<IActionResult> RequestJoinEvent(string postId, [FromBody] JoinParticipantDto joinRequest)
        {
            await _participantService.AddParticipantAsync(postId, joinRequest);

            return Ok(new { message = "Join request sent successfully, waiting for host approval." });
        }

        [HttpPut("{postId}/confirm-participant/{userId}")]
        public async Task<IActionResult> ConfirmParticipant(string postId, string userId)
        {
            // Katılımcının durumunu güncelle
            var success = await _participantService.UpdateParticipantStatus(postId, userId);

            if (!success)
            {
                return NotFound(new { message = "Participant or post not found." });
            }

            return Ok(new { message = "Participant confirmed successfully." });
        }


    }
}