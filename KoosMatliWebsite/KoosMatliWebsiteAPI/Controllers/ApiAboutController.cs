using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AboutController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public AboutController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<SchoolInformation>> GetSchoolInformation()
        {
            var information =
                await _context.SchoolInformation.FirstOrDefaultAsync();

            if (information == null)
            {
                return NotFound();
            }

            return information;
        }

        [HttpPost]
        public async Task<ActionResult<SchoolInformation>> CreateSchoolInformation(
            SchoolInformation information)
        {
            information.Id = 0;

            _context.SchoolInformation.Add(information);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSchoolInformation),
                new { id = information.Id },
                information);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolInformation(
            int id,
            SchoolInformation information)
        {
            if (id != information.Id)
            {
                return BadRequest();
            }

            _context.Entry(information).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
