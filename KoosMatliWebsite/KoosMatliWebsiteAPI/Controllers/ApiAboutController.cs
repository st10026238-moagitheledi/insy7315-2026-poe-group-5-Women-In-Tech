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

        // GET: api/About
        [HttpGet]
        public async Task<ActionResult<SchoolInformation>> GetSchoolInformation()
        {
            var information =
                await _context.SchoolInformation.FirstOrDefaultAsync();

            if (information == null)
            {
                return NotFound();
            }

            return Ok(information);
        }


        // POST: api/About
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


        // PUT: api/About/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolInformation(
            int id,
            SchoolInformation information)
        {
            if (id != information.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request.");
            }

            var existingInformation =
                await _context.SchoolInformation.FindAsync(id);

            if (existingInformation == null)
            {
                return NotFound();
            }

            existingInformation.SchoolName = information.SchoolName;
            existingInformation.Motto = information.Motto;
            existingInformation.Description = information.Description;
            existingInformation.History = information.History;
            existingInformation.Mission = information.Mission;
            existingInformation.Vision = information.Vision;
            existingInformation.Address = information.Address;
            existingInformation.Phone = information.Phone;
            existingInformation.Email = information.Email;
            existingInformation.PrincipalName = information.PrincipalName;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}