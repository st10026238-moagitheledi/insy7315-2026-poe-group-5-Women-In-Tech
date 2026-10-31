using KoosMatliApp.Features.Teacher.Models;

#if ANDROID
using Plugin.Firebase.Auth;
using Plugin.Firebase.Core.Exceptions;
using Plugin.Firebase.Firestore;
#endif

namespace KoosMatliApp.Features.Teacher.Services;

public class FirebaseAuthService
{
    private readonly TeacherApiService _teacherApiService;


    public FirebaseAuthService(
        TeacherApiService teacherApiService)
    {
        _teacherApiService =
            teacherApiService;
    } // (Microsoft, 2026)


    public async Task<FirebaseLoginResult> SignInAsync(
        string email,
        string password)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            return new FirebaseLoginResult
            {
                Success = false,
                Message =
                    "Please enter your email address and password."
            };
        }


#if ANDROID

        try
        {
            var cleanEmail =
                email.Trim();


            // Sign in with Firebase Authentication.
            await CrossFirebaseAuth.Current
                .SignInWithEmailAndPasswordAsync(
                    cleanEmail,
                    password); // (Microsoft, 2026)


            var currentUser =
                CrossFirebaseAuth.Current.CurrentUser;


            if (currentUser == null)
            {
                return new FirebaseLoginResult
                {
                    Success = false,
                    Message =
                        "Firebase could not identify the signed-in user."
                };
            }


            var userId =
                currentUser.Uid;


            // Read users/{UID} from Firestore.
            var document =
                await CrossFirebaseFirestore.Current
                    .GetCollection("users")
                    .GetDocument(userId)
                    .GetDocumentSnapshotAsync
                        <FirebaseUserRecord>();


            var userRecord =
                document.Data; // (Microsoft, 2026)


            if (userRecord == null)
            {
                await CrossFirebaseAuth.Current
                    .SignOutAsync();


                return new FirebaseLoginResult
                {
                    Success = false,
                    Message =
                        "Your account exists, but no user profile was found."
                };
            }


            if (!userRecord.IsActive)
            {
                await CrossFirebaseAuth.Current
                    .SignOutAsync();


                return new FirebaseLoginResult
                {
                    Success = false,
                    Message =
                        "This account is currently inactive."
                };
            }

            if (string.Equals(
                    userRecord.Role,
                    "teacher",
                    StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    await _teacherApiService
                        .LoginAsync(
                            cleanEmail,
                            password);
                }
                catch (Exception apiException)
                {
                    await CrossFirebaseAuth.Current
                        .SignOutAsync();


                    _teacherApiService
                        .ClearSession();


                    return new FirebaseLoginResult
                    {
                        Success = false,
                        Message =
                            $"Teacher API login failed: {apiException.Message}"
                    };
                }
            } // (Microsoft, 2026)


            return new FirebaseLoginResult
            {
                Success = true,
                Message =
                    "Login successful.",

                Role =
                    userRecord.Role,

                UserId =
                    userId,

                DisplayName =
                    userRecord.DisplayName,

                Email =
                    userRecord.Email
            };
        }
        catch (CrossPlatformFirebaseAuthException ex)
        {
            _teacherApiService
                .ClearSession();


            var firebaseMessage =
                ex.NativeErrorMessage;


            if (string.IsNullOrWhiteSpace(
                    firebaseMessage))
            {
                firebaseMessage =
                    "Invalid email address or password.";
            }


            return new FirebaseLoginResult
            {
                Success = false,
                Message =
                    firebaseMessage
            };
        }
        catch (Exception ex)
        {
            _teacherApiService
                .ClearSession();


            return new FirebaseLoginResult
            {
                Success = false,
                Message =
                    $"Login failed: {ex.Message}"
            }; // (Microsoft, 2026)
        }

#else

        await Task.CompletedTask;


        return new FirebaseLoginResult
        {
            Success = false,
            Message =
                "Firebase login is currently configured for Android."
        };

#endif
    }


    public async Task SignOutAsync()
    {
        _teacherApiService
            .ClearSession();


#if ANDROID

        await CrossFirebaseAuth.Current
            .SignOutAsync();

#else

        await Task.CompletedTask;

#endif
    } // (Microsoft, 2026)
}


//Reference List:
//1. Microsoft, 2026. Introduction to .NET MAUI. [online] Available at: https://learn.microsoft.com/en-us/dotnet/architecture/maui/introduction [Accessed 31 October 2026].

