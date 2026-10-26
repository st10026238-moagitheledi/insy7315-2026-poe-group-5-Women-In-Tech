using Microsoft.AspNetCore.Mvc;
using KoosMatliAdminApi.Data;
using KoosMatliAdminApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliAdminApi.Controllers
{
    [ApiController]
    [Route("api/admin/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocumentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/admin/documents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolDocument>>> GetAll()
        {
            var docs = await _context.SchoolDocuments.ToListAsync();
            return Ok(docs);
        }

        // GET: api/admin/documents/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolDocument>> Get(int id)
        {
            var doc = await _context.SchoolDocuments.FindAsync(id);
            if (doc == null) return NotFound();
            return Ok(doc);
        }
    }
}
