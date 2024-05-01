using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Domain.Entities
{
    [FirestoreData]
    public class User
    {
        [JsonPropertyName("userID")]
        public string UserID { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("fullname")]
        public string FullName { get; set; }

        [JsonPropertyName("isHost")]
        public bool IsHost { get; set; }

        [JsonPropertyName("passwordHash")]
        public string PasswordHash { get; set; }

        [JsonPropertyName("profilePicture")]
        public string ProfilePicture { get; set; }

        [JsonPropertyName("about")]
        public string About { get; set; }
    }
}