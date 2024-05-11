
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Newtonsoft.Json;
using SoframiPaylas.Infrastructure.Data.Config;
using Polly;
using Google.Api.Gax.Grpc;

namespace SoframiPaylas.Infrastructure.Data.Service
{
    public class FirebaseService
    {
        private readonly FirestoreDb db;

        public FirebaseService(FireBaseConfig config)
        {
            string jsonConfig = JsonConvert.SerializeObject(new
            {
                type = config.Type,
                project_id = config.ProjectId,
                private_key_id = config.PrivateKeyId,
                private_key = config.PrivateKey,
                client_email = config.ClientEmail,
                client_id = config.ClientId,
                auth_uri = config.AuthUri,
                token_uri = config.TokenUri,
                auth_provider_x509_cert_url = config.AuthProviderX509CertUrl,
                client_x509_cert_url = config.ClientX509CertUrl
            });

            // GoogleCredential nesnesini JSON string'den oluştur
            var credential = GoogleCredential.FromJson(jsonConfig).CreateScoped(FirestoreClient.DefaultScopes);
            var firestoreClient = new FirestoreClientBuilder
            {
                ChannelCredentials = credential.ToChannelCredentials(),

            }.Build();
            db = FirestoreDb.Create("sofrani-paylas", firestoreClient);
        }
        public FirestoreDb GetDb()
        {
            return db;
        }
        /*public async Task<T> ExecuteFirestoreOperationAsync<T>(Func<Task<T>> operation, TimeSpan timeout)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4)
                });

            var timeoutPolicy = Policy.TimeoutAsync(timeout, Polly.Timeout.TimeoutStrategy.Pessimistic);

            var policyWrap = Policy.WrapAsync(retryPolicy, timeoutPolicy);

            return await policyWrap.ExecuteAsync(operation);
        }*/

    }
}