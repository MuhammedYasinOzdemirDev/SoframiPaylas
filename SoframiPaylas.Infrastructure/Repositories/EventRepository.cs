using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly FirebaseService _service;
        public EventRepository(FirebaseService firebaseService)
        {
            _service = firebaseService;
        }

        public async Task<string> CreateEventAsync(Event Event)
        {
            string eventId = Guid.NewGuid().ToString();
            await _service.GetDb().Collection("Events").Document(eventId).SetAsync(Event);
            return eventId;
        }

        public async Task<List<Event>> GetEventAllAsync()
        {
            if (_service == null || _service.GetDb() == null)
                throw new InvalidOperationException("Database service is not initialized properly.");
            CollectionReference eventRef = _service.GetDb().Collection("Events");
            QuerySnapshot snapshots = await eventRef.GetSnapshotAsync();

            List<Event> events = new List<Event>();
            foreach (DocumentSnapshot document in snapshots.Documents)
            {
                if (document.Exists)
                {
                    Dictionary<string, object> eventDict = document.ToDictionary();
                    var eventItem = new Event
                    {
                        HostID = eventDict.ContainsKey("hostID") ? eventDict["hostID"]?.ToString() : null,
                        Title = eventDict.ContainsKey("title") ? eventDict["title"]?.ToString() : null,
                        Description = eventDict.ContainsKey("description") ? eventDict["description"]?.ToString() : null,
                        Location = eventDict.ContainsKey("location") && eventDict["location"] is GeoPoint ? (GeoPoint)eventDict["location"] : new GeoPoint(0, 0),
                        Date = eventDict.ContainsKey("date") && eventDict["date"] is Timestamp ? (Timestamp)eventDict["date"] : Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc)),
                        Time = eventDict.ContainsKey("time") ? eventDict["time"]?.ToString() : null,
                        ParticipantIDs = eventDict.ContainsKey("participantIDs") && eventDict["participantIDs"] is List<object>
                ? ((List<object>)eventDict["participantIDs"]).Cast<string>().ToList()
                : new List<string>(),
                        MaxParticipants = eventDict.ContainsKey("maxParticipants") ? Convert.ToInt32(eventDict["maxParticipants"]) : 0,
                        Images = eventDict.ContainsKey("images") ? eventDict["images"]?.ToString() : null,
                        EventStatus = eventDict.ContainsKey("eventStatus") ? Convert.ToBoolean(eventDict["eventStatus"]) : false,
                    };

                    events.Add(eventItem);
                }
            }

            return events;
        }
    }
}