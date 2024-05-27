using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Polly;
using SoframiPaylas.Infrastructure.Data.Config;

namespace SoframiPaylas.Infrastructure.Data.Service
{
    public class FirebaseAuthService
    {
        private readonly FirebaseAuth auth;

        public FirebaseAuthService(FirebaseAppProvider appProvider)
        {
            auth = FirebaseAuth.GetAuth(appProvider.GetFirebaseApp());
        }
        public FirebaseAuth GetAuth()
        {
            return auth;
        }
    }
}