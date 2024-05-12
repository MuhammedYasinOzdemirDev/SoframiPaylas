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
        public FirebaseAuthService(FireBaseConfig config)
        {
            string jsonConfig = JsonSerializer.Serialize(new
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
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(jsonConfig)
            });
            auth = FirebaseAuth.DefaultInstance;
        }
        public FirebaseAuth GetAuth()
        {
            return auth;
        }
    }
}