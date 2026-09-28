using Microsoft.AspNetCore.Mvc;
using KoosMatliWebsite.Models;

namespace KoosMatliWebsite.Controllers
{
    [ApiController]
    [Route("api/admissions")]
    public class AdmissionsApiController : ControllerBase
    {
        private static readonly List<AdmissionApplication> Applications = new();

        [HttpGet]
        public ActionResult<IEnumerable<AdmissionApplication>> GetApplications()
        {
            return Ok(Applications);
        }

        [HttpGet("{id}")]
        public ActionResult<AdmissionApplication> GetApplication(int id)
        {
            var application = Applications.FirstOrDefault(a => a.Id == id);

            if (application == null)
            {

                return NotFound(new
                {
                    success = false,
                    message = "No application found"
                });
            }
            return Ok(application);
        }

        [HttpPost]
        public ActionResult<AdmissionApplication> SubmitApplication([FromBody] AdmissionApplication application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            application.Id = Applications.Count == 0 ? 1 : Applications.Max(a => a.Id) + 1;
            application.AppicationDate = DateTime.Now;
            application.Status = "Pending";

            Applications.Add(application);

            return CreatedAtAction(nameof(GetApplication),
                new { id = application.Id },
                new
                {
                    success = true,
                    message = "Applications submitted successfully",
                    application
                });

        }
    }
}
