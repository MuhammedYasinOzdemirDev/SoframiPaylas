using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public AuthRepository(FirebaseAuthService authService, FirebaseService dbservice)
        {
            auth = authService.GetAuth();
            db = dbservice.GetDb();
        }
        public async Task<string> RegisterUserAsync(User user, string password)
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
        }
        public async Task<string> GenerateEmailVerificationLink(string email)
        {
            var user = await auth.GetUserByEmailAsync(email);

            var link = await auth.GenerateEmailVerificationLinkAsync(email, new ActionCodeSettings
            {
                Url = "http://localhost:5103/api/auth",
                HandleCodeInApp = true,
            });
            return link;

        }
    }
}