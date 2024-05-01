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
    }
}