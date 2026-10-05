using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffMembersController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public StaffMembersController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // =========================================================
        // GET: api/staffmembers
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetStaffMembers()
        {
            var database = _firebaseService.Database;

            var snapshot = await database
                .Collection("staffMembers")
                .OrderBy("Id")
                .GetSnapshotAsync();

            var staffMembers = snapshot.Documents
                .Select(MapStaffMember)
                .ToList();

            return Ok(staffMembers);
        }

        // =========================================================
        // GET: api/staffmembers/{id}
        // =========================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStaffMember(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("staffMembers")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Staff member with ID {id} was not found.");
            }

            var staffMember = MapStaffMember(snapshot);

            return Ok(staffMember);
        }

        // =========================================================
        // POST: api/staffmembers
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> CreateStaffMember(
            StaffMember staffMember)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("staffMembers")
                .Document(staffMember.Id.ToString());

            var existingSnapshot =
                await documentReference.GetSnapshotAsync();

            if (existingSnapshot.Exists)
            {
                return Conflict(
                    $"A staff member with ID {staffMember.Id} already exists.");
            }

            var data = StaffMemberToDictionary(staffMember);

            await documentReference.SetAsync(data);

            return CreatedAtAction(
                nameof(GetStaffMember),
                new { id = staffMember.Id },
                staffMember);
        }

        // =========================================================
        // PUT: api/staffmembers/{id}
        // =========================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStaffMember(
            int id,
            StaffMember updatedStaffMember)
        {
            if (id != updatedStaffMember.Id)
            {
                return BadRequest(
                    "The ID in the URL does not match the ID in the request.");
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("staffMembers")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Staff member with ID {id} was not found.");
            }

            var data = StaffMemberToDictionary(updatedStaffMember);

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return Ok(new
            {
                message = "Staff member updated successfully.",
                staffMember = updatedStaffMember
            });
        }

        // =========================================================
        // DELETE: api/staffmembers/{id}
        // =========================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStaffMember(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("staffMembers")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Staff member with ID {id} was not found.");
            }

            await documentReference.DeleteAsync();

            return Ok(new
            {
                message = "Staff member deleted successfully."
            });
        }

        // =========================================================
        // Helper: Convert Firestore document to StaffMember
        // =========================================================

        private static StaffMember MapStaffMember(
            DocumentSnapshot document)
        {
            return new StaffMember
            {
                Id = GetInt(document, "Id"),
                FullName = GetString(document, "FullName"),
                Position = GetString(document, "Position"),
                Department = GetString(document, "Department"),
                Category = GetString(document, "Category"),
                Biography = GetString(document, "Biography"),
                ImageUrl = GetString(document, "ImageUrl"),
                Email = GetString(document, "Email"),
                IsActive = GetBool(document, "IsActive")
            };
        }

        // =========================================================
        // Helper: Convert StaffMember to Firestore dictionary
        // =========================================================

        private static Dictionary<string, object> StaffMemberToDictionary(
            StaffMember staffMember)
        {
            return new Dictionary<string, object>
            {
                { "Id", staffMember.Id },
                { "FullName", staffMember.FullName },
                { "Position", staffMember.Position },
                { "Department", staffMember.Department },
                { "Category", staffMember.Category },
                { "Biography", staffMember.Biography },
                { "ImageUrl", staffMember.ImageUrl },
                { "Email", staffMember.Email },
                { "IsActive", staffMember.IsActive }
            };
        }

        // =========================================================
        // Firestore Helper Methods
        // =========================================================

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
    }
}