using Google.Cloud.Firestore;

namespace SoframiPaylas.Domain.Entities
{
    [FirestoreData]
    public class Post
    {
        [FirestoreProperty("hostID")]
        public string HostID { get; set; }

        [FirestoreProperty("title")]
        public string Title { get; set; }

        [FirestoreProperty("description")]
        public string Description { get; set; }

        [FirestoreProperty("location")]
        public GeoPoint Location { get; set; }

        [FirestoreProperty("date")]
        public Timestamp Date { get; set; }

        [FirestoreProperty("time")]
        public string Time { get; set; }

        [FirestoreProperty("maxParticipants")]
        public int MaxParticipants { get; set; }

        [FirestoreProperty("image")]
        public string Image { get; set; }

        [FirestoreProperty("eventStatus")]
        public bool PostStatus { get; set; }

        // Related Foods (Yemekler)
        [FirestoreProperty("relatedFoods")]
        public List<string> RelatedFoods { get; set; }

        // Participants with status
        [FirestoreProperty("participants")]
        public List<string> Participants { get; set; }
    }
}