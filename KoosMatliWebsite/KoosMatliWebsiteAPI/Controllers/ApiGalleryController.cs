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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GalleryItem>>> GetGallery()
        {
            return await _context.GalleryItems
                .Where(g => g.IsPublished)
                .OrderByDescending(g => g.DateAdded)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GalleryItem>> GetGalleryItem(int id)
        {
            var item = await _context.GalleryItems.FindAsync(id);

            if (item == null || !item.IsPublished)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<GalleryItem>> CreateGalleryItem(
            GalleryItem item)
        {
            item.Id = 0;

            if (item.DateAdded == default)
            {
                item.DateAdded = DateTime.UtcNow;
            }

            _context.GalleryItems.Add(item);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetGalleryItem),
                new { id = item.Id },
                item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGalleryItem(
            int id,
            GalleryItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State =
                EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

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
    }
}
