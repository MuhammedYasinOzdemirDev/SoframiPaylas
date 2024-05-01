using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoframiPaylas.Domain.Entities
{
    public class Event
    {
        public int EventID { get; set; }
        public int HostID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public List<int> ParticipantIDs { get; set; }
        public int MaxParticipants { get; set; }
        public string Images { get; set; }
        public string EventStatus { get; set; }

        // Navigation properties
        public User Host { get; set; }
        public List<User> Participants { get; set; }
    }
}