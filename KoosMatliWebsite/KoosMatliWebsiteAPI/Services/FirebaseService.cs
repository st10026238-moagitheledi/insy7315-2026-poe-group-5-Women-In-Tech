using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Grpc.Auth;
using System.Text.Json;

namespace KoosMatliWebsiteAPI.Services
{
    public class FirebaseService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirebaseService(IConfiguration configuration)
        {
            var credentialPath = configuration["Firebase:CredentialPath"];

            if (string.IsNullOrWhiteSpace(credentialPath))
            {
                throw new InvalidOperationException(
                    "Firebase credential path has not been configured.");
            }

            credentialPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                credentialPath);

            if (!File.Exists(credentialPath))
            {
                throw new FileNotFoundException(
                    "Firebase service account file was not found.",
                    credentialPath);
            }

            // Read the Firebase project ID from the service-account JSON.
            var json = File.ReadAllText(credentialPath);

            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty(
                    "project_id",
                    out var projectIdElement))
            {
                throw new InvalidOperationException(
                    "The Firebase service account JSON does not contain a project_id.");
            }

            var projectId = projectIdElement.GetString();

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new InvalidOperationException(
                    "Firebase project ID is empty.");
            }

            // Load the service-account credentials.
            var credential = GoogleCredential
                .FromFile(credentialPath)
                .CreateScoped(
                    "https://www.googleapis.com/auth/datastore");

            // Initialise Firebase Admin SDK.
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
            }

            // Explicitly give Firestore the service-account credentials.
            var firestoreBuilder = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                ChannelCredentials = credential.ToChannelCredentials()
            };

            _firestoreDb = firestoreBuilder.Build();
        }

        public FirestoreDb Database => _firestoreDb;
    }
}