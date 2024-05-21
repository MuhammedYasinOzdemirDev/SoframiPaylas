namespace SoframiPaylas.WebUI.Models;
public class CreatePostViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public string FormattedDate { get; set; }
    public string Time { get; set; }
    public int MaxParticipants { get; set; }
    public string ImageUrl { get; set; }
}
