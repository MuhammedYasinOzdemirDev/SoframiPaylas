using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly FirestoreDb db;
        private readonly FirebaseService firebaseService;
        public UserRepository(FirebaseService service)
        {
            db = service.GetDb();
            firebaseService = service;
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
                {
                    if (db == null)
                    {
                        throw new InvalidOperationException("Database service is not initialized properly.");
                    }

                    CollectionReference usersRef = db.Collection("Users");
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
                                UserName = userDict.ContainsKey("userName") ? userDict["userName"].ToString() : null,
                                IsHost = userDict.ContainsKey("isHost") ? (bool)userDict["isHost"] : false,
                                ProfilePicture = userDict.ContainsKey("profilePicture") ? userDict["profilePicture"].ToString() : null,
                                About = userDict.ContainsKey("about") ? userDict["about"].ToString() : null,
                                Role = userDict.ContainsKey("role") ? userDict["role"].ToString() : null,
                                Name = userDict.ContainsKey("name") ? userDict["name"].ToString() : null,
                                Surname = userDict.ContainsKey("surname") ? userDict["surname"].ToString() : null,
                                Phone = userDict.ContainsKey("phone") ? userDict["phone"].ToString() : null
                            };
                            users.Add(user);
                        }
                    }
                    return users;
                }, TimeSpan.FromSeconds(20));
        }
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (db == null)
                {
                    throw new InvalidOperationException("Database service is not initialized.");
                }
                DocumentReference eventRef = db.Collection("Users").Document(userId);
                DocumentSnapshot snapshot = await eventRef.GetSnapshotAsync();


                if (!snapshot.Exists)
                {
                    return null;  // Eğer belge yoksa null döner
                }

                Dictionary<string, object> userDict = snapshot.ToDictionary();
                return new User
                {
                    Email = userDict.ContainsKey("email") ? userDict["email"].ToString() : null,
                    UserName = userDict.ContainsKey("userName") ? userDict["userName"].ToString() : null,
                    IsHost = userDict.ContainsKey("isHost") ? (bool)userDict["isHost"] : false,
                    ProfilePicture = userDict.ContainsKey("profilePicture") ? userDict["profilePicture"].ToString() : null,
                    About = userDict.ContainsKey("about") ? userDict["about"].ToString() : null
                    ,
                    Role = userDict.ContainsKey("role") ? userDict["role"].ToString() : null,
                    Name = userDict.ContainsKey("name") ? userDict["name"].ToString() : null,
                    Surname = userDict.ContainsKey("surname") ? userDict["surname"].ToString() : null,
                    Phone = userDict.ContainsKey("phone") ? userDict["phone"].ToString() : null
                };

            }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> UpdateUserAsync(User user, string userId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user), "User object must not be null.");

                DocumentReference userRef = db.Collection("Users").Document(userId);

                // Firestore ile kullanıcı verilerini güncelleme
                try
                {
                    await userRef.SetAsync(user, SetOptions.MergeAll);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }, TimeSpan.FromSeconds(20));
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
           {
               if (string.IsNullOrEmpty(userId))
                   throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

               DocumentReference userRef = db.Collection("Users").Document(userId);

               // Firestore'dan belirtilen kullanıcıyı silme
               try
               {
                   await userRef.DeleteAsync(); return true;
               }
               catch (Exception ex)
               {
                   return false;
               }
           }, TimeSpan.FromSeconds(20));
        }
    }
}