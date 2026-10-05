using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public AboutController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // =========================================================
        // GET: api/about
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetSchoolInformation()
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("schoolInformation")
                .Document("main");

            var snapshot = await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound("School information was not found.");
            }

            var schoolInformation = new SchoolInformation
            {
                Id = GetInt(snapshot, "Id"),
                SchoolName = GetString(snapshot, "SchoolName"),
                Motto = GetString(snapshot, "Motto"),
                Description = GetString(snapshot, "Description"),
                History = GetString(snapshot, "History"),
                Mission = GetString(snapshot, "Mission"),
                Vision = GetString(snapshot, "Vision"),
                Address = GetString(snapshot, "Address"),
                Phone = GetString(snapshot, "Phone"),
                Email = GetString(snapshot, "Email"),
                PrincipalName = GetString(snapshot, "PrincipalName")
            };

            return Ok(schoolInformation);
        }

        // =========================================================
        // POST: api/about
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateSchoolInformation(
            SchoolInformation schoolInformation)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("schoolInformation")
                .Document("main");

            var existingSnapshot =
                await documentReference.GetSnapshotAsync();

            if (existingSnapshot.Exists)
            {
                return Conflict(
                    "School information already exists. Use PUT to update it.");
            }

            var data = new Dictionary<string, object>
            {
                { "Id", schoolInformation.Id },
                { "SchoolName", schoolInformation.SchoolName },
                { "Motto", schoolInformation.Motto },
                { "Description", schoolInformation.Description },
                { "History", schoolInformation.History },
                { "Mission", schoolInformation.Mission },
                { "Vision", schoolInformation.Vision },
                { "Address", schoolInformation.Address },
                { "Phone", schoolInformation.Phone },
                { "Email", schoolInformation.Email },
                { "PrincipalName", schoolInformation.PrincipalName }
            };

            await documentReference.SetAsync(data);

            return Ok(new
            {
                message = "School information created successfully.",
                schoolInformation
            });
        }

        // =========================================================
        // PUT: api/about/{id}
        // =========================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSchoolInformation(
            int id,
            SchoolInformation updatedInformation)
        {
            if (id != updatedInformation.Id)
            {
                return BadRequest(
                    "The ID in the URL does not match the ID in the request.");
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("schoolInformation")
                .Document("main");

            var snapshot = await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    "School information was not found.");
            }

            var data = new Dictionary<string, object>
            {
                { "Id", id },
                { "SchoolName", updatedInformation.SchoolName },
                { "Motto", updatedInformation.Motto },
                { "Description", updatedInformation.Description },
                { "History", updatedInformation.History },
                { "Mission", updatedInformation.Mission },
                { "Vision", updatedInformation.Vision },
                { "Address", updatedInformation.Address },
                { "Phone", updatedInformation.Phone },
                { "Email", updatedInformation.Email },
                { "PrincipalName", updatedInformation.PrincipalName }
            };

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return Ok(new
            {
                message = "School information updated successfully.",
                schoolInformation = updatedInformation
            });
        }

        // =========================================================
        // Helper Methods
        // =========================================================

        private static string GetString(
            DocumentSnapshot snapshot,
            string field)
        {
            if (!snapshot.ContainsField(field))
            {
                return "";
            }

            return snapshot.GetValue<string>(field) ?? "";
        }

        private static int GetInt(
            DocumentSnapshot snapshot,
            string field)
        {
            if (!snapshot.ContainsField(field))
            {
                return 0;
            }

            return snapshot.GetValue<int>(field);
        }
    }
}