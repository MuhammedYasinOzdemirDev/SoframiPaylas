using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.Text.Json;
using SoframiPaylas.Infrastructure.Data.Config;

namespace SoframiPaylas.Infrastructure.Data.Service;
public class FirebaseAppProvider
{
    private readonly FirebaseApp _firebaseApp;

    public FirebaseAppProvider(FireBaseConfig config)
    {
        if (FirebaseApp.DefaultInstance == null)
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

            _firebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromJson(jsonConfig)
            });
        }
        else
        {
            _firebaseApp = FirebaseApp.DefaultInstance;
        }
    }

    public FirebaseApp GetFirebaseApp()
    {
        return _firebaseApp;
    }
}

