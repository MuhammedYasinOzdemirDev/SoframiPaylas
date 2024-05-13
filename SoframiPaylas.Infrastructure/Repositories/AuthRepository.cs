using System.Text;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;


        public AuthRepository(FirebaseAuthService authService, FirebaseService dbservice)
        {
            auth = authService.GetAuth();
            db = dbservice.GetDb();
            firebaseService = dbservice;
            _httpClient = new HttpClient();
            _apiKey = "AIzaSyDKNXm3TMii0A6r3hvE0cSU2_zjusP-vjI";
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
                                    Url = "http://localhost:5162/Auth/EmailConfirmed",
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
        public async Task<string> SignInWithEmailAndPassword(string email, string password)
        {
            return await firebaseService.ExecuteFirestoreOperationAsync(async () =>
            {
                var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_apiKey}";

                var requestData = new
                {
                    email = email,
                    password = password,
                    returnSecureToken = true
                };

                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return jsonResponse; // Bu JSON yanıt, ID token dahil olmak üzere kullanıcı bilgilerini içerir.
                }
                else
                {
                    throw new Exception("Authentication failed: " + response.ReasonPhrase);
                }
            }, TimeSpan.FromSeconds(20));
        }
        public async Task<FirebaseUser> GetUserDetailsAsync(string idToken)
        {
            var requestUri = $"https://identitytoolkit.googleapis.com/v1/accounts:lookup?key={_apiKey}";
            var requestData = new { idToken };
            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(requestUri, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve user data: " + response.ReasonPhrase);
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(jsonResponse);
            return JsonConvert.DeserializeObject<List<FirebaseUser>>(JObject.Parse(jsonResponse)["users"].ToString()).FirstOrDefault();
        }
    }
}