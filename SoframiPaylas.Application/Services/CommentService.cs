using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs.Comment;
using SoframiPaylas.Application.Interfaces;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Application.Services;
public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(string postId)
    {
        var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
        return comments.Select(c =>
        {
            var dto = _mapper.Map<CommentDto>(c.comment);
            dto.Id = c.commentId; return dto;
        }
        );
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto commentDto)
    {
        var comment = _mapper.Map<Comment>(commentDto);
        comment.CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow);
        var result = await _commentRepository.AddCommentAsync(comment);
        var dto = _mapper.Map<CommentDto>(result.comment);
        dto.Id = result.commentId;
        return dto;
    }

    public async Task<bool> DeleteCommentAsync(string commentId)
    {
        var comment = await _commentRepository.GetCommentsByPostIdAsync(commentId);
        if (comment == null)
        {
            throw new KeyNotFoundException("Comment not found.");
        }
        return await _commentRepository.DeleteCommentAsync(commentId);
    }
}
