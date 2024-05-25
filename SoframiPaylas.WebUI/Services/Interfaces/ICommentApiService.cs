using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Application.DTOs.Comment;

namespace SoframiPaylas.WebUI.Services.Interfaces
{
    public interface ICommentApiService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId);
        Task<HttpResponseMessage> AddCommentAsync(CreateCommentDto commentDto);
        Task<HttpResponseMessage> DeleteCommentAsync(string commentId);

    }
}