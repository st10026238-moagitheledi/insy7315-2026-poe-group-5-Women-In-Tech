using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public GalleryController(WebsiteDbContext context)
        {
            _context = context;
        }


        // =========================================================
        // GET: api/gallery
        // =========================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GalleryItem>>> GetGallery()
        {
            return await _context.GalleryItems
                .Where(g => g.IsPublished)
                .OrderByDescending(g => g.DateAdded)
                .ToListAsync();
        }


        // =========================================================
        // GET: api/gallery/{id}
        // =========================================================

        [HttpGet("{id}")]
        public async Task<ActionResult<GalleryItem>> GetGalleryItem(int id)
        {
            var item = await _context.GalleryItems
                .FirstOrDefaultAsync(g =>
                    g.Id == id &&
                    g.IsPublished);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }


        // =========================================================
        // POST: api/gallery
        // =========================================================

        [HttpPost]
        public async Task<ActionResult<GalleryItem>> CreateGalleryItem(
            GalleryItem item)
        {
            item.Id = 0;

            if (item.DateAdded == default)
            {
                item.DateAdded = DateTime.UtcNow;
            }

            if (string.IsNullOrWhiteSpace(item.Category))
            {
                item.Category = "general";
            }

            item.Category = item.Category.ToLower();

            _context.GalleryItems.Add(item);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetGalleryItem),
                new { id = item.Id },
                item);
        }


        // =========================================================
        // PUT: api/gallery/{id}
        // =========================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGalleryItem(
            int id,
            GalleryItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(item.Category))
            {
                item.Category = "general";
            }

            item.Category = item.Category.ToLower();

            _context.Entry(item).State =
                EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GalleryItemExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }


        // =========================================================
        // DELETE: api/gallery/{id}
        // =========================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGalleryItem(int id)
        {
            var item = await _context.GalleryItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.GalleryItems.Remove(item);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // =========================================================
        // CHECK IF GALLERY ITEM EXISTS
        // =========================================================

        private bool GalleryItemExists(int id)
        {
            return _context.GalleryItems.Any(e => e.Id == id);
        }
    }
}