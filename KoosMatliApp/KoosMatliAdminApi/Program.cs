using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using KoosMatliAdminApi.Auth;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Localization;
using KoosMatliAdminApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args); //(Microsoft Learn, 2026a).

// Read the Firebase project ID from configuration.
var projectId = builder.Configuration["Firebase:ProjectId"]?.Trim();

if (string.IsNullOrWhiteSpace(projectId) ||
    projectId.StartsWith("REPLACE_", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException(
        "Set Firebase:ProjectId in appsettings.json " +
        "to the exact Project ID shown in Firebase Console.");
}

if (projectId.Any(char.IsWhiteSpace))
{
    throw new InvalidOperationException(
        "Firebase:ProjectId must not contain spaces. " +
        "Copy the Project ID, not the project display name.");
}

var firestoreEmulatorHost =
    Environment.GetEnvironmentVariable("FIRESTORE_EMULATOR_HOST");

var authEmulatorHost =
    Environment.GetEnvironmentVariable("FIREBASE_AUTH_EMULATOR_HOST");

var usingFirestoreEmulator =
    !string.IsNullOrWhiteSpace(firestoreEmulatorHost);

var usingAuthEmulator =
    !string.IsNullOrWhiteSpace(authEmulatorHost);

if (!builder.Environment.IsDevelopment() &&
    (usingFirestoreEmulator || usingAuthEmulator))
{
    throw new InvalidOperationException(
        "Firebase emulators are permitted only in Development.");
}

// Localization.
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultures = Languages.Codes
        .Select(code => new CultureInfo(code))
        .ToArray();

    options.DefaultRequestCulture = new RequestCulture("en-ZA");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
});

// Controllers and translated validation messages.
builder.Services.AddScoped<LocalizedResultFilter>();

builder.Services
    .AddControllers(options =>
    {
        options.Filters.AddService<LocalizedResultFilter>();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var messages = context.HttpContext.RequestServices
                .GetRequiredService<IStringLocalizer<Messages>>();

            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => new[] { messages["InvalidInput"].Value });

            return new BadRequestObjectResult(
                new ValidationProblemDetails(errors)
                {
                    Title = messages["InvalidInput"].Value
                });
        };
    });

// Cloud Firestore.
builder.Services.AddSingleton<FirestoreDb>(_ => //(Google Cloud, 2026b).
{
    return new FirestoreDbBuilder //(Google Cloud, 2026b).
    {
        ProjectId = projectId,
        EmulatorDetection = EmulatorDetection.EmulatorOrProduction
    }.Build();
});

builder.Services.AddSingleton<FirestoreDataStore>();

// Backend token verification.
// This does not implement the frontend sign-in process.
builder.Services.AddSingleton<FirebaseApp>(_ =>
{
    var credential =
        builder.Environment.IsDevelopment() && usingAuthEmulator
            ? GoogleCredential.FromAccessToken("emulator")
            : GoogleCredential.GetApplicationDefault();

    return FirebaseApp.Create(new AppOptions
    {
        ProjectId = projectId,
        Credential = credential
    });
});

builder.Services.AddSingleton<FirebaseAuth>(services =>
{
    var firebaseApp = services.GetRequiredService<FirebaseApp>();
    return FirebaseAuth.GetAuth(firebaseApp);
});

builder.Services
    .AddAuthentication("Firebase")
    .AddScheme<
        AuthenticationSchemeOptions,
        FirebaseAuthenticationHandler>(
            "Firebase",
            _ => { }); //(Microsoft Learn, 2026b).

builder.Services.AddAuthorization();

// CORS is relevant to browser clients.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? Array.Empty<string>();

        if (origins.Length > 0)
        {
            policy
                .WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("Content-Language");
        }
    });
});

var app = builder.Build();

app.UseRequestLocalization(); //(Microsoft Learn, 2026c; Microsoft Learn, 2026d).
app.UseMiddleware<ApiExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Optional demo data, disabled unless explicitly configured.
if (app.Environment.IsDevelopment() &&
    builder.Configuration.GetValue<bool>("Firebase:SeedDemoData"))
{
    await DataSeeder.SeedAsync(
        app.Services.GetRequiredService<FirestoreDataStore>());
}

await app.RunAsync();

/*Reference List
 
 Microsoft Corporation, "ASP.NET Core App Startup and Configuration," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/startup
 Google LLC, "Initialize the SDK in .NET," Firebase Documentation, 2026. [Online]. Available: https://firebase.google.com/docs/admin/setup
 Google Cloud, "Cloud Firestore .NET Client Library Documentation," Google Cloud Documentation, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/
 Microsoft Corporation, "Overview of ASP.NET Core Authentication," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/
 Microsoft Corporation, "Globalize and Localize ASP.NET Core," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization
 Microsoft Corporation, "Handle Errors in ASP.NET Core Web APIs," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors
 */