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
        [FirestoreProperty("userID")]
        public string UserID { get; set; }
        [FirestoreProperty("title")]
        public string Title { get; set; }
        [FirestoreProperty("description")]
        public string Description { get; set; }
        [FirestoreProperty("date")]
        public Timestamp Date { get; set; }
        [FirestoreProperty("participants")]
        public int Participants { get; set; }
        [FirestoreProperty("ımages")]
        public string Images { get; set; }
        [FirestoreProperty("status")]
        public string Status { get; set; }
    }
}