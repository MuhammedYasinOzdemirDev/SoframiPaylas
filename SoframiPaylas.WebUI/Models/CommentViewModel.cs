namespace SoframiPaylas.WebUI.Models;
public class CommentViewModel
{
    public string Id { get; set; }
    public string PostId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
