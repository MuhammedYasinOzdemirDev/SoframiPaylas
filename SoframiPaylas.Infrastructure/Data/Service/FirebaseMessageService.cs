using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using SoframiPaylas.Infrastructure.Data.Config;

namespace SoframiPaylas.Infrastructure.Data.Service
{
    public class FirebaseMessagingService
    {
        private readonly FirebaseMessaging messaging;

        public FirebaseMessagingService(FirebaseAppProvider appProvider)
        {
            messaging = FirebaseMessaging.GetMessaging(appProvider.GetFirebaseApp());
        }

        public FirebaseMessaging GetMessaging()
        {
            return messaging;
        }

        public async Task SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Token = token
            };

            string response = await messaging.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}
