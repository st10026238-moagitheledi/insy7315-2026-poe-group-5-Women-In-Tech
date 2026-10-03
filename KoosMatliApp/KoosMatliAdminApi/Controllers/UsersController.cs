using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Models;

namespace KoosMatliAdminApi.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        // GET: api/admin/users/role/Parent
        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByRole(string role)
        {
            var users = await _context.Users
                .Where(u => u.Role.ToLower() == role.ToLower())
                .ToListAsync();

            return Ok(users);
        }

        // GET: api/admin/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}