using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class SchoolEventsController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public SchoolEventsController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }


        // =====================================================
        // GET ALL EVENTS
        // GET: api/events
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var database = _firebaseService.Database;

            var snapshot = await database
                .Collection("events")
                .OrderBy("Date")
                .GetSnapshotAsync();

            var events = snapshot.Documents
                .Select(MapSchoolEvent)
                .ToList();

            return Ok(events);
        }


        // =====================================================
        // GET EVENT BY ID
        // GET: api/events/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("events")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"School event with ID {id} was not found.");
            }

            var schoolEvent = MapSchoolEvent(snapshot);

            return Ok(schoolEvent);
        }


        // =====================================================
        // CREATE EVENT
        // POST: api/events
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateEvent(
            SchoolEvent schoolEvent)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("events")
                .Document(schoolEvent.Id.ToString());

            var existingSnapshot =
                await documentReference.GetSnapshotAsync();

            if (existingSnapshot.Exists)
            {
                return Conflict(
                    $"A school event with ID {schoolEvent.Id} already exists.");
            }

            var data = SchoolEventToDictionary(schoolEvent);

            await documentReference.SetAsync(data);

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = schoolEvent.Id },
                schoolEvent);
        }


        // =====================================================
        // UPDATE EVENT
        // PUT: api/events/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEvent(
            int id,
            SchoolEvent updatedEvent)
        {
            if (id != updatedEvent.Id)
            {
                return BadRequest(
                    "The ID in the URL does not match the ID in the request.");
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("events")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"School event with ID {id} was not found.");
            }

            var data = SchoolEventToDictionary(updatedEvent);

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return Ok(new
            {
                message = "School event updated successfully.",
                schoolEvent = updatedEvent
            });
        }


        // =====================================================
        // DELETE EVENT
        // DELETE: api/events/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("events")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"School event with ID {id} was not found.");
            }

            await documentReference.DeleteAsync();

            return Ok(new
            {
                message = "School event deleted successfully."
            });
        }


        // =====================================================
        // FIRESTORE → SCHOOL EVENT MODEL
        // =====================================================

        private static SchoolEvent MapSchoolEvent(
            DocumentSnapshot document)
        {
            return new SchoolEvent
            {
                Id = GetInt(
                    document,
                    "Id"),

                Title = GetString(
                    document,
                    "Title"),

                Description = GetString(
                    document,
                    "Description"),

                Date = GetDateTime(
                    document,
                    "Date"),

                Category = GetString(
                    document,
                    "Category"),

                Time = GetString(
                    document,
                    "Time"),

                Location = GetString(
                    document,
                    "Location"),

                IsPublished = GetBool(
                    document,
                    "IsPublished")
            };
        }


        // =====================================================
        // SCHOOL EVENT MODEL → FIRESTORE
        // =====================================================

        private static Dictionary<string, object> SchoolEventToDictionary(
            SchoolEvent schoolEvent)
        {
            return new Dictionary<string, object>
            {
                { "Id", schoolEvent.Id },

                { "Title", schoolEvent.Title },

                { "Description", schoolEvent.Description },

                {
                    "Date",
                    Timestamp.FromDateTime(
                        schoolEvent.Date.ToUniversalTime())
                },

                { "Category", schoolEvent.Category },

                { "Time", schoolEvent.Time },

                { "Location", schoolEvent.Location },

                { "IsPublished", schoolEvent.IsPublished }
            };
        }


        // =====================================================
        // FIRESTORE FIELD HELPERS
        // =====================================================

        private static string GetString(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return "";
            }

            return document.GetValue<string>(field) ?? "";
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


        private static bool GetBool(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return true;
            }

            return document.GetValue<bool>(field);
        }


        private static DateTime GetDateTime(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return DateTime.MinValue;
            }

            var timestamp =
                document.GetValue<Timestamp>(field);

            return timestamp.ToDateTime();
        }
    }
}