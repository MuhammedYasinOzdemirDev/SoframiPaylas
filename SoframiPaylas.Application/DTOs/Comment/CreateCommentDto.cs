namespace SoframiPaylas.Application.DTOs.Comment;
public class CreateCommentDto
{
    public string PostId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Content { get; set; }
}
