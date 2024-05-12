/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;*/
using Google.Cloud.Firestore;


namespace SoframiPaylas.Domain.Entities
{
    public enum ParticipationStatus
    {
        Pending, // Beklemede
        Confirmed, // Onaylandı
        Declined // Reddedildi
    }
    [FirestoreData]
    public class Participant
    {
        [FirestoreProperty("userID")]
        public string UserID { get; set; }
        [FirestoreProperty("postID")]
        public string PostId { get; set; }

        [FirestoreProperty("status")]
        public int Status { get; set; } = (int)ParticipationStatus.Pending;

    }
}