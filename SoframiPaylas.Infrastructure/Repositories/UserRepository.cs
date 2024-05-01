using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
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
                        UserID = userDict["userID"].ToString(),
                        Email = userDict["email"].ToString(),
                        FullName = userDict["fullname"].ToString(),
                        IsHost = (bool)userDict["isHost"],
                        PasswordHash = userDict["passwordHash"].ToString(),
                        ProfilePicture = userDict["profilePicture"].ToString(),
                        About = userDict["about"].ToString()
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

    }
}