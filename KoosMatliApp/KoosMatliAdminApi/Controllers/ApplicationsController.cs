using Microsoft.AspNetCore.Mvc;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Models;
using KoosMatliAdminApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliAdminApi.Controllers
{
    [ApiController]
    [Route("api/admin/applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdmissionApplication>>> GetAll()
        {
            var apps = await _context.Applications.ToListAsync();
            return Ok(apps);
        }

        // GET: api/admin/applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdmissionApplication>> Get(int id)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();
            return Ok(app);
        }

        // PUT: api/admin/applications/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateApplicationStatusDto dto)
        {
            var app = await _context.Applications.FindAsync(id);
            if (app == null) return NotFound();

            app.Status = dto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
