using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface ICommentApiService
    {
        Task<HttpResponseMessage> GetCommentsByPostIdAsync(string postId);
        Task<HttpResponseMessage> AddCommentAsync(CreateCommentViewModel createPostViewModel);
        Task<HttpResponseMessage> DeleteCommentAsync(string commentId);

    }
}