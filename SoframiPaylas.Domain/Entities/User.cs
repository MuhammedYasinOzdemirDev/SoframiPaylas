using Google.Cloud.Firestore;

namespace SoframiPaylas.Domain.Entities
{
    [FirestoreData]
    public class User
    {

        [FirestoreProperty("email")]
        public string Email { get; set; }
        [FirestoreProperty("name")]
        public string Name { get; set; }
        [FirestoreProperty("surname")]
        public string Surname { get; set; }

        [FirestoreProperty("userName")]
        public string UserName { get; set; }

        [FirestoreProperty("isHost")]
        public bool IsHost { get; set; }


        [FirestoreProperty("profilePicture")]
        public string ProfilePicture { get; set; }

        [FirestoreProperty("about")]
        public string About { get; set; }
        [FirestoreProperty("role")]
        public string Role { get; set; }
        [FirestoreProperty("phone")]
        public string Phone { get; set; }
    }
}