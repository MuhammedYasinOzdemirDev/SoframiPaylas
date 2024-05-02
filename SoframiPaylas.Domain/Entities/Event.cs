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

        [FirestoreProperty("EventID")]
        public int EventID { get; set; }
        [FirestoreProperty("HostID")]
        public int HostID { get; set; }
        [FirestoreProperty("Title")]
        public string Title { get; set; }
        [FirestoreProperty("Description")]
        public string Description { get; set; }
        [FirestoreProperty("Location")]
        public string Location { get; set; }
        [FirestoreProperty("Date")]
        public Timestamp Date { get; set; }
        [FirestoreProperty("Time")]
        public string Time { get; set; }
        [FirestoreProperty("participantIDs")]
        public List<string> ParticipantIDs { get; set; }
        [FirestoreProperty("MaxParticipants")]
        public int MaxParticipants { get; set; }
        [FirestoreProperty("Images")]
        public string Images { get; set; }
        [FirestoreProperty("EventStatus")]
        public string EventStatus { get; set; }
    }
}