using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdmissionsController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public AdmissionsController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admission>>> GetAdmissions()
        {
            return await _context.Admissions
                .Where(a => a.IsActive)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Admission>> GetAdmission(int id)
        {
            var admission =
                await _context.Admissions.FindAsync(id);

            if (admission == null || !admission.IsActive)
            {
                return NotFound();
            }

            return admission;
        }

        [HttpPost]
        public async Task<ActionResult<Admission>> CreateAdmission(
            Admission admission)
        {
            admission.Id = 0;

            _context.Admissions.Add(admission);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAdmission),
                new { id = admission.Id },
                admission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmission(
            int id,
            Admission admission)
        {
            if (id != admission.Id)
            {
                return BadRequest();
            }

            _context.Entry(admission).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmission(int id)
        {
            var admission =
                await _context.Admissions.FindAsync(id);

            if (admission == null)
            {
                return NotFound();
            }

            _context.Admissions.Remove(admission);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
