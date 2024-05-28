using SoframiPaylas.Application.DTOs.Comment;

namespace SoframiPaylas.Application.Interfaces;
public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId);
    Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto);
    Task<bool> DeleteCommentAsync(string commentId);
    Task<int> CommentCount(string userId);
}