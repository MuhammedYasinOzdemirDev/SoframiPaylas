using Google.Cloud.Firestore;

namespace SoframiPaylas.WebUI.Models;
public class PostViewModel
{
    public string HostID { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
    public GeoPoint Location { get; set; }
    public string FormattedDate { get; set; }
    public string Time { get; set; }
    public int MaxParticipants { get; set; }
    public string Image { get; set; }
    public bool PostStatus { get; set; }
    public List<string> RelatedFoods { get; set; }

    public List<string> Participants { get; set; }
}