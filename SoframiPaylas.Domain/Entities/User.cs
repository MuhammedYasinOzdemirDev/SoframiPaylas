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
        [FirestoreProperty("userID")]
        public string UserID { get; set; }

        [FirestoreProperty("email")]
        public string Email { get; set; }

        [FirestoreProperty("fullname")]
        public string FullName { get; set; }

        [FirestoreProperty("isHost")]
        public bool IsHost { get; set; }

        [FirestoreProperty("passwordHash")]
        public string PasswordHash { get; set; }

        [FirestoreProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [FirestoreProperty("about")]
        public string About { get; set; }
    }
}