using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/Applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public ApplicationsController(
            FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // =====================================================
        // GET: api/Applications
        // Get all applications
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            var database = _firebaseService.Database;

            var snapshot = await database
                .Collection("applications")
                .GetSnapshotAsync();

            var applications = snapshot.Documents
                .Select(MapApplication)
                .OrderByDescending(a => a.DateSubmitted)
                .ToList();

            return Ok(applications);
        }


        // =====================================================
        // GET: api/Applications/{id}
        // Get one application by ID
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetApplication(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("applications")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }

            var application = MapApplication(snapshot);

            return Ok(application);
        }


        // =====================================================
        // POST: api/Applications
        // Submit a new application
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateApplication(
            Application application)
        {
            var database = _firebaseService.Database;

            // Make sure the API controls these values
            application.Status = "Pending";
            application.DateSubmitted = DateTime.UtcNow;

            // Generate the next numeric ID
            var snapshot = await database
                .Collection("applications")
                .GetSnapshotAsync();

            var nextId = snapshot.Documents
                .Select(document => GetInt(document, "Id"))
                .DefaultIfEmpty(0)
                .Max() + 1;

            application.Id = nextId;

            // Create Firestore document
            var documentReference = database
                .Collection("applications")
                .Document(application.Id.ToString());

            var data = ApplicationToDictionary(application);

            await documentReference.SetAsync(data);

            return CreatedAtAction(
                nameof(GetApplication),
                new { id = application.Id },
                application);
        }


        // =====================================================
        // PUT: api/Applications/{id}
        // Update an existing application
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateApplication(
            int id,
            Application application)
        {
            // Make sure the URL ID matches the application ID
            if (id != application.Id)
            {
                return BadRequest(new
                {
                    message =
                        "The ID in the URL does not match the ID in the request."
                });
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("applications")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }

            // Get the existing application
            var existingApplication = MapApplication(snapshot);

            // =================================================
            // Parent / Guardian Information
            // =================================================

            existingApplication.ParentFirstName =
                application.ParentFirstName;

            existingApplication.ParentLastName =
                application.ParentLastName;

            existingApplication.ParentEmail =
                application.ParentEmail;

            existingApplication.ParentPhone =
                application.ParentPhone;

            existingApplication.RelationshipToLearner =
                application.RelationshipToLearner;


            // =================================================
            // Learner Information
            // =================================================

            existingApplication.LearnerFirstName =
                application.LearnerFirstName;

            existingApplication.LearnerLastName =
                application.LearnerLastName;

            existingApplication.LearnerDateOfBirth =
                application.LearnerDateOfBirth;

            existingApplication.GradeApplyingFor =
                application.GradeApplyingFor;

            existingApplication.PreviousSchool =
                application.PreviousSchool;


            // =================================================
            // Address
            // =================================================

            existingApplication.HomeAddress =
                application.HomeAddress;


            // =================================================
            // Additional Information
            // =================================================

            existingApplication.AdditionalInformation =
                application.AdditionalInformation;


            // =================================================
            // Application Status
            // =================================================

            existingApplication.Status =
                application.Status;


            // Keep the original ID and submission date
            existingApplication.Id = id;


            var data = ApplicationToDictionary(
                existingApplication);

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return NoContent();
        }


        // =====================================================
        // DELETE: api/Applications/{id}
        // Delete an application
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("applications")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }

            await documentReference.DeleteAsync();

            return NoContent();
        }


        // =====================================================
        // Map Firestore document → Application
        // =====================================================

        private static Application MapApplication(
            DocumentSnapshot document)
        {
            return new Application
            {
                Id = GetInt(document, "Id"),

                ParentFirstName =
                    GetString(document, "ParentFirstName"),

                ParentLastName =
                    GetString(document, "ParentLastName"),

                ParentEmail =
                    GetString(document, "ParentEmail"),

                ParentPhone =
                    GetString(document, "ParentPhone"),

                RelationshipToLearner =
                    GetString(document, "RelationshipToLearner"),

                LearnerFirstName =
                    GetString(document, "LearnerFirstName"),

                LearnerLastName =
                    GetString(document, "LearnerLastName"),

                LearnerDateOfBirth =
                    GetDateTime(
                        document,
                        "LearnerDateOfBirth"),

                GradeApplyingFor =
                    GetString(document, "GradeApplyingFor"),

                PreviousSchool =
                    GetString(document, "PreviousSchool"),

                HomeAddress =
                    GetString(document, "HomeAddress"),

                AdditionalInformation =
                    GetString(
                        document,
                        "AdditionalInformation"),

                Status =
                    GetString(document, "Status"),

                DateSubmitted =
                    GetDateTime(
                        document,
                        "DateSubmitted")
            };
        }


        // =====================================================
        // Map Application → Firestore dictionary
        // =====================================================

        private static Dictionary<string, object>
            ApplicationToDictionary(
                Application application)
        {
            return new Dictionary<string, object>
            {
                {
                    "Id",
                    application.Id
                },

                {
                    "ParentFirstName",
                    application.ParentFirstName
                },

                {
                    "ParentLastName",
                    application.ParentLastName
                },

                {
                    "ParentEmail",
                    application.ParentEmail
                },

                {
                    "ParentPhone",
                    application.ParentPhone
                },

                {
                    "RelationshipToLearner",
                    application.RelationshipToLearner
                },

                {
                    "LearnerFirstName",
                    application.LearnerFirstName
                },

                {
                    "LearnerLastName",
                    application.LearnerLastName
                },

                {
                    "LearnerDateOfBirth",
                    Timestamp.FromDateTime(
                        application.LearnerDateOfBirth
                            .ToUniversalTime())
                },

                {
                    "GradeApplyingFor",
                    application.GradeApplyingFor
                },

                {
                    "PreviousSchool",
                    application.PreviousSchool
                },

                {
                    "HomeAddress",
                    application.HomeAddress
                },

                {
                    "AdditionalInformation",
                    application.AdditionalInformation
                },

                {
                    "Status",
                    application.Status
                },

                {
                    "DateSubmitted",
                    Timestamp.FromDateTime(
                        application.DateSubmitted
                            .ToUniversalTime())
                }
            };
        }


        // =====================================================
        // Firestore helper methods
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


        private static DateTime GetDateTime(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return DateTime.MinValue;
            }

            return document
                .GetValue<Timestamp>(field)
                .ToDateTime();
        }
    }
}