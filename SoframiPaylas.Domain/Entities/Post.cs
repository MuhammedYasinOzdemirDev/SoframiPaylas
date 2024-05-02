using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Domain.Entities
{
    [FirestoreData]
    public class Post
    {
        [FirestoreProperty("PostId")]
        public string PostID { get; set; }
        [FirestoreProperty("userID")]
        public string UserID { get; set; }
        [FirestoreProperty("Title")]
        public string Title { get; set; }
        [FirestoreProperty("Description")]
        public string Description { get; set; }
        [FirestoreProperty("Date")]
        public DateTime Date { get; set; }
        [FirestoreProperty("Participants")]
        public int Participants { get; set; }
        [FirestoreProperty("Images")]
        public string Images { get; set; }
        [FirestoreProperty("Status")]
        public string Status { get; set; }
    }
}