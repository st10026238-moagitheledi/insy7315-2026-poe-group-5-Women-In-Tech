using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public ContactController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(
            ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var database = _firebaseService.Database;

            // Generate the next numeric ID
            var snapshot = await database
                .Collection("contactMessages")
                .GetSnapshotAsync();

            var nextId = snapshot.Documents
                .Select(document => GetInt(document, "Id"))
                .DefaultIfEmpty(0)
                .Max() + 1;

            message.Id = nextId;

            // Set submission time
            message.SubmittedAt = DateTime.UtcNow;

            // Create Firestore document
            var documentReference = database
                .Collection("contactMessages")
                .Document(message.Id.ToString());

            var data = new Dictionary<string, object>
            {
                { "Id", message.Id },
                { "Name", message.Name },
                { "Email", message.Email },
                { "Subject", message.Subject },
                { "Message", message.Message },
                {
                    "SubmittedAt",
                    Timestamp.FromDateTime(
                        message.SubmittedAt.ToUniversalTime())
                },
                { "IsRead", message.IsRead }
            };

            await documentReference.SetAsync(data);

            return Ok(new
            {
                message = "Your message has been sent successfully."
            });
        }

        private static int GetInt(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return 0;
            }

            return document.GetValue<int>(field);
        }
    }
}