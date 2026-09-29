using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffMembersController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public StaffMembersController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffMember>>> GetStaffMembers()
        {
            return await _context.StaffMembers
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StaffMember>> GetStaffMember(int id)
        {
            var staffMember = await _context.StaffMembers.FindAsync(id);

            if (staffMember == null || !staffMember.IsActive)
            {
                return NotFound();
            }

            return staffMember;
        }

        [HttpPost]
        public async Task<ActionResult<StaffMember>> CreateStaffMember(
            StaffMember staffMember)
        {
            _context.StaffMembers.Add(staffMember);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetStaffMember),
                new { id = staffMember.Id },
                staffMember);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaffMember(
            int id,
            StaffMember staffMember)
        {
            if (id != staffMember.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffMember).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffMember(int id)
        {
            var staffMember =
                await _context.StaffMembers.FindAsync(id);

            if (staffMember == null)
            {
                return NotFound();
            }

            _context.StaffMembers.Remove(staffMember);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
