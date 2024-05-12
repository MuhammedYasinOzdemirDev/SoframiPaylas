using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using SoframiPaylas.Domain.Entities;
using SoframiPaylas.Infrastructure.Data.Service;
using SoframiPaylas.Infrastructure.Interfaces;

namespace SoframiPaylas.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly FirebaseAuth auth;
        private readonly FirestoreDb db;
        private readonly FirebaseService firebaseService;
        public AuthRepository(FirebaseAuthService authService, FirebaseService dbservice)
        {
            auth = authService.GetAuth();
            db = dbservice.GetDb();
            firebaseService = dbservice;
        }
        public async Task<string> RegisterUserAsync(User user, string password)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                var userRecord = await auth.CreateUserAsync(new UserRecordArgs
                {
                    Email = user.Email,
                    Password = password,
                    DisplayName = user.UserName,
                    EmailVerified = false
                });
                await auth.SetCustomUserClaimsAsync(userRecord.Uid, new Dictionary<string, object>{
        {"role","user"}
           });

                var userDocRef = db.Collection("Users").Document(userRecord.Uid);
                await userDocRef.SetAsync(user);

                return userRecord.Uid;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<string> GenerateEmailVerificationLink(string email)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                var user = await auth.GetUserByEmailAsync(email);

                var link = await auth.GenerateEmailVerificationLinkAsync(email, new ActionCodeSettings
                {
                    Url = "http://localhost:5103/api/auth",
                    HandleCodeInApp = true,
                });
                return link;
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<bool> GetUserByUsernameAsync(string username)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                var usersRef = db.Collection("Users");
                var querySnapshot = await usersRef.WhereEqualTo("userName", username).GetSnapshotAsync();
                foreach (var document in querySnapshot.Documents)
                {
                    if (document.Exists)
                    {
                        return true;
                    }
                }
                return false;

            }, TimeSpan.FromSeconds(20));
        }
    }
}