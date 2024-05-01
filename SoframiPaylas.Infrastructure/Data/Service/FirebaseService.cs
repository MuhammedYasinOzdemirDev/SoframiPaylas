using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace SoframiPaylas.Infrastructure.Data.Service
{
    public class FirebaseService
    {
        private FirebaseApp _app;
        private FirebaseClient _client;

        public FirebaseService()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                _app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("path/to/your/firebase-service-account-file.json")
                });
            }

            _client = new FirebaseClient("https://your-firebase-database-url.firebaseio.com/");
        }

        public FirebaseClient GetClient()
        {
            return _client;
        }
    }
}