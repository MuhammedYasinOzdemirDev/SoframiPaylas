using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SoframiPaylas.Infrastructure.Data.Config;

namespace SoframiPaylas.Infrastructure.Data.Service
{
    public class FirebaseService
    {
        private readonly FirestoreDb db;
        private readonly FireBaseConfig _config;


        public FirebaseService(FireBaseConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config), "FirebaseConfig cannot be null."); ;
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
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();
            db = FirestoreDb.Create("sofrani-paylas", firestoreClient);
        }
        public FirestoreDb GetDb()
        {
            return db;
        }

    }
}