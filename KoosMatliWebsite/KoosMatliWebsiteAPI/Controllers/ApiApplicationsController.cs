using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationsController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public ApplicationsController(WebsiteDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET: api/Applications
        // Get all applications
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            var applications = await _context.Applications
                .OrderByDescending(a => a.DateSubmitted)
                .ToListAsync();

            return Ok(applications);
        }


        // =====================================================
        // GET: api/Applications/
        // Get one application by ID
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _context.Applications
                .FindAsync(id);

            if (application == null)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }

            return Ok(application);
        }


        // =====================================================
        // POST: api/Applications
        // Submit a new application
        // =====================================================

        [HttpPost]
        public async Task<ActionResult<Application>> CreateApplication(
            Application application)
        {
            // Make sure the API controls these values
            application.Id = 0;
            application.Status = "Pending";
            application.DateSubmitted = DateTime.UtcNow;

            _context.Applications.Add(application);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetApplication),
                new { id = application.Id },
                application);
        }


        // =====================================================
        // PUT: api/Applications/
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
                    message = "The ID in the URL does not match the ID in the request."
                });
            }

            var existingApplication =
                await _context.Applications.FindAsync(id);

            if (existingApplication == null)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }


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


            await _context.SaveChangesAsync();

            return NoContent();
        }


        // =====================================================
        // DELETE: api/Applications/
        // Delete an application
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application =
                await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound(new
                {
                    message = "Application not found."
                });
            }

            _context.Applications.Remove(application);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
