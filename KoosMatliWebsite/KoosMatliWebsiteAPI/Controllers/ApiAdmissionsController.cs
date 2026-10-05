using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/admissions")]
    [ApiController]
    public class AdmissionsController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public AdmissionsController(
            FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }


        // =====================================================
        // GET ALL ACTIVE ADMISSIONS
        // GET: api/admissions
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAdmissions()
        {
            var database = _firebaseService.Database;

            var snapshot = await database
                .Collection("admissions")
                .WhereEqualTo("IsActive", true)
                .OrderBy("Id")
                .GetSnapshotAsync();

            var admissions = snapshot.Documents
                .Select(MapAdmission)
                .ToList();

            return Ok(admissions);
        }


        // =====================================================
        // GET ADMISSION BY ID
        // GET: api/admissions/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAdmission(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("admissions")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Admission with ID {id} was not found.");
            }

            var admission = MapAdmission(snapshot);

            if (!admission.IsActive)
            {
                return NotFound(
                    $"Admission with ID {id} is not active.");
            }

            return Ok(admission);
        }


        // =====================================================
        // CREATE ADMISSION
        // POST: api/admissions
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateAdmission(
            Admission admission)
        {
            var database = _firebaseService.Database;

            // Generate the next numeric ID.
            var snapshot = await database
                .Collection("admissions")
                .GetSnapshotAsync();

            var nextId = snapshot.Documents
                .Select(document => GetInt(document, "Id"))
                .DefaultIfEmpty(0)
                .Max() + 1;

            admission.Id = nextId;

            var documentReference = database
                .Collection("admissions")
                .Document(admission.Id.ToString());

            var data = AdmissionToDictionary(admission);

            await documentReference.SetAsync(data);

            return CreatedAtAction(
                nameof(GetAdmission),
                new { id = admission.Id },
                admission);
        }


        // =====================================================
        // UPDATE ADMISSION
        // PUT: api/admissions/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAdmission(
            int id,
            Admission admission)
        {
            if (id != admission.Id)
            {
                return BadRequest(
                    "The ID in the URL does not match the ID in the request.");
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("admissions")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Admission with ID {id} was not found.");
            }

            var data = AdmissionToDictionary(admission);

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return Ok(new
            {
                message = "Admission updated successfully.",
                admission
            });
        }


        // =====================================================
        // DELETE ADMISSION
        // DELETE: api/admissions/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAdmission(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("admissions")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Admission with ID {id} was not found.");
            }

            await documentReference.DeleteAsync();

            return Ok(new
            {
                message = "Admission deleted successfully."
            });
        }


        // =====================================================
        // FIRESTORE → ADMISSION MODEL
        // =====================================================

        private static Admission MapAdmission(
            DocumentSnapshot document)
        {
            return new Admission
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

                Requirements = GetString(
                    document,
                    "Requirements"),

                ApplicationInstructions = GetString(
                    document,
                    "ApplicationInstructions"),

                ApplicationLink = GetString(
                    document,
                    "ApplicationLink"),

                IsActive = GetBool(
                    document,
                    "IsActive")
            };
        }


        // =====================================================
        // ADMISSION MODEL → FIRESTORE
        // =====================================================

        private static Dictionary<string, object> AdmissionToDictionary(
            Admission admission)
        {
            return new Dictionary<string, object>
            {
                { "Id", admission.Id },
                { "Title", admission.Title },
                { "Description", admission.Description },
                { "Requirements", admission.Requirements },
                { "ApplicationInstructions", admission.ApplicationInstructions },
                { "ApplicationLink", admission.ApplicationLink },
                { "IsActive", admission.IsActive }
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
    }
}