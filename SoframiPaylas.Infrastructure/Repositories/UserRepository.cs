using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Firebase.Database;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly FirebaseService _service;
        public UserRepository(FirebaseService service)
        {
            _service = service;
        }


        public async Task<string> CreateUserAsync(User user)

        {

            string userId = Guid.NewGuid().ToString();
            await _service.GetDb().Collection("Users").Document(userId).SetAsync(user);

            return userId;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            if (_service == null || _service.GetDb() == null)
            {
                throw new InvalidOperationException("Database service is not initialized properly.");
            }

            CollectionReference usersRef = _service.GetDb().Collection("Users");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();


            List<User> users = new List<User>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    Dictionary<string, object> userDict = document.ToDictionary();
                    var user = new User
                    {
                        Email = userDict.ContainsKey("email") ? userDict["email"].ToString() : null,
                        FullName = userDict.ContainsKey("fullname") ? userDict["fullname"].ToString() : null,
                        IsHost = userDict.ContainsKey("isHost") ? (bool)userDict["isHost"] : false,
                        PasswordHash = userDict.ContainsKey("passwordHash") ? userDict["passwordHash"].ToString() : null,
                        ProfilePicture = userDict.ContainsKey("profilePicture") ? userDict["profilePicture"].ToString() : null,
                        About = userDict.ContainsKey("about") ? userDict["about"].ToString() : null
                    };
                    users.Add(user);
                }
            }
            return users;
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            if (_service == null || _service.GetDb() == null)
            {
                throw new InvalidOperationException("Database service is not initialized.");
            }
            Query query = _service.GetDb().Collection("Users").WhereEqualTo("userID", userId);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();


            DocumentSnapshot document = snapshot.Documents[0];

            if (!document.Exists)
            {
                return null;  // Eğer belge yoksa null döner
            }

            Dictionary<string, object> userDict = document.ToDictionary();
            return new User
            {

                Email = userDict["email"].ToString(),
                FullName = userDict["fullname"].ToString(),
                IsHost = Convert.ToBoolean(userDict["isHost"]),
                PasswordHash = userDict["passwordHash"].ToString(),
                ProfilePicture = userDict["profilePicture"].ToString(),
                About = userDict["about"].ToString()
            };

        }

        public async Task UpdateUserAsync(User user, string userId)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User object must not be null.");



            DocumentReference userRef = _service.GetDb().Collection("Users").Document(userId);

            // Firestore ile kullanıcı verilerini güncelleme
            await userRef.SetAsync(user, SetOptions.MergeAll);
        }

        public async Task DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            DocumentReference userRef = _service.GetDb().Collection("Users").Document(userId);

            // Firestore'dan belirtilen kullanıcıyı silme
            await userRef.DeleteAsync();
        }
    }
}