using KoosMatliApp.Features.Teacher.Models;

#if ANDROID
using Plugin.Firebase.Auth;
using Plugin.Firebase.Core.Exceptions;
using Plugin.Firebase.Firestore;
#endif

namespace KoosMatliApp.Features.Teacher.Services;

public class FirebaseAuthService
{
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
            // Sign in with Firebase Authentication
            await CrossFirebaseAuth.Current
                .SignInWithEmailAndPasswordAsync(
                    email.Trim(),
                    password);


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


            // Read users/{UID} from Firestore
            var document =
                await CrossFirebaseFirestore.Current
                    .GetCollection("users")
                    .GetDocument(userId)
                    .GetDocumentSnapshotAsync<FirebaseUserRecord>();


            var userRecord =
                document.Data;


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
            return new FirebaseLoginResult
            {
                Success = false,
                Message =
                    $"Login failed: {ex.Message}"
            };
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
#if ANDROID

        await CrossFirebaseAuth.Current
            .SignOutAsync();

#else

        await Task.CompletedTask;

#endif
    }
}