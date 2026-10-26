using Microsoft.AspNetCore.Mvc;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliAdminApi.Controllers
{
    [ApiController]
    [Route("api/admin/staff")]
    public class StaffController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StaffController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/staff
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffMember>>> GetAll()
        {
            var staff = await _context.StaffMembers.ToListAsync();
            return Ok(staff);
        }

        // GET: api/admin/staff/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffMember>> Get(int id)
        {
            var member = await _context.StaffMembers.FindAsync(id);
            if (member == null) return NotFound();
            return Ok(member);
        }
    }
}
