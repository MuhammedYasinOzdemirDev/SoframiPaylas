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

        [FirestoreProperty("email")]
        public string Email { get; set; }

        [FirestoreProperty("fullName")]
        public string FullName { get; set; }

        [FirestoreProperty("isHost")]
        public bool IsHost { get; set; }


        [FirestoreProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [FirestoreProperty("about")]
        public string About { get; set; }
    }
}