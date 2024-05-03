using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Application.DTOs.Event
{
    public class UpdateEventDto
    {

        public string HostID { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public GeoPoint Location { get; set; }

        public Timestamp Date { get; set; }

        public string Time { get; set; }

        public List<string> ParticipantIDs { get; set; }
        public int MaxParticipants { get; set; }

        public string Images { get; set; }
        public bool EventStatus { get; set; }
    }
}