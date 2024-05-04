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
            try
            {
                var posts = await _postservice.GetAllPostsAsync();
                if (posts == null || !posts.Any())
                    return NotFound("Post not found");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                // Log the exception details here
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetPostByIdAsync(string postId)
        {
            try
            {
                var post = await _postservice.GetPostByIdAsync(postId);
                if (post == null)
                    return NotFound("Post not found");
                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }
        [HttpPost("post")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto postDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var postId = await _postservice.CreatePostAsync(postDto);
                if (postId == null)
                    return BadRequest("Failed to create post.");
                return Ok(postId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }

        [HttpPut("post")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDto postDto, string postId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updateResult = await _postservice.UpdatePostAsync(postId, postDto);
                if (!updateResult)
                    return NotFound("Post to update not found.");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }

        [HttpDelete("post")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            try
            {
                var deleteResult = await _postservice.DeletePostAsync(postId);
                if (!deleteResult)
                    return NotFound("Post to delete not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }

        [HttpPost("{postId}/join")]
        public async Task<IActionResult> RequestJoinEvent(string postId, [FromBody] JoinParticipantDto joinRequest)
        {
            try
            {
                var joinResult = await _participantService.AddParticipantAsync(postId, joinRequest);
                if (!joinResult)
                    return BadRequest("Failed to send join request.");
                return Ok(new { message = "Join request sent successfully, waiting for host approval." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }

        [HttpPut("{postId}/confirm-participant/{userId}")]
        public async Task<IActionResult> ConfirmParticipant(string postId, string userId)
        {
            try
            {
                // Katılımcının durumunu güncelle
                var success = await _participantService.UpdateParticipantStatus(postId, userId);

                if (!success)
                {
                    return NotFound(new { message = "Participant or post not found." });
                }

                return Ok(new { message = "Participant confirmed successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the post.");
            }
        }
    }
}