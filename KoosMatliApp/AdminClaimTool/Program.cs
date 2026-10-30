using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

if (args.Length != 1 || string.IsNullOrWhiteSpace(args[0]))
{
    Console.WriteLine(
        "Run: dotnet run --project AdminClaimTool -- \"YOUR_UID\"");

    return;
}

var app = FirebaseApp.Create(new AppOptions
{
    ProjectId = "koosmatliprimaryschoolsystem",
    Credential = GoogleCredential.GetApplicationDefault()
});

var auth = FirebaseAuth.GetAuth(app);

var user = await auth.GetUserAsync(args[0].Trim());

var claims = user.CustomClaims.ToDictionary(
    claim => claim.Key,
    claim => claim.Value);

claims["admin"] = true;

await auth.SetCustomUserClaimsAsync(user.Uid, claims);

Console.WriteLine(
    $"Administrator permission assigned to {user.Email}.");

Console.WriteLine(
    "Sign in again to receive the updated permission.");