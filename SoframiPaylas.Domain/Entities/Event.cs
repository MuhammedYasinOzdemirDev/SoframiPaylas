using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Domain.Entities
{
    [FirestoreData]
    public class Event
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
        [FirestoreProperty("participantIDs")]
        public List<string> ParticipantIDs { get; set; }
        [FirestoreProperty("maxParticipants")]
        public int MaxParticipants { get; set; }
        [FirestoreProperty("images")]
        public string Images { get; set; }
        [FirestoreProperty("eventStatus")]
        public bool EventStatus { get; set; }
    }
}