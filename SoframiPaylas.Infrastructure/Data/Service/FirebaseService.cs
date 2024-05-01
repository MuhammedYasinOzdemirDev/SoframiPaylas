using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace SoframiPaylas.Infrastructure.Data.Service
{
    public class FirebaseService
    {
        private readonly FirestoreDb db;

        string pathToYourJson = "C:\\Users\\User\\Desktop\\SoframiPaylas\\SoframiPaylas.Infrastructure\\Data\\Config\\sofrani-paylas-firebase.json";


        public FirebaseService()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathToYourJson);
            db = FirestoreDb.Create("sofrani-paylas");
        }
        public FirestoreDb GetDb()
        {
            return db;
        }
    }
}