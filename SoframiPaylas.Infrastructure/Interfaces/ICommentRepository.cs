using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Infrastructure.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<(Comment comment, string commentId)>> GetCommentsByPostIdAsync(string postId);
        Task<(Comment comment, string commentId)> AddCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(string commentId);
        Task<int> CommentCount(string userId);
    }
}