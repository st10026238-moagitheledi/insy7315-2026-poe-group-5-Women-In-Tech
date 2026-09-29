using KoosMatliWebsiteAPI.Data;
using KoosMatliWebsiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public NewsController(WebsiteDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            return await _context.News
                .Where(n => n.IsPublished)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            var news = await _context.News
                .FirstOrDefaultAsync(n =>
                    n.Id == id &&
                    n.IsPublished);

            if (news == null)
            {
                return NotFound();
            }

            return news;
        }

        [HttpPost]
        public async Task<ActionResult<News>> CreateNews(News news)
        {
            news.Id = 0;

            if (news.PublishedDate == default)
            {
                news.PublishedDate = DateTime.UtcNow;
            }

            if (string.IsNullOrWhiteSpace(news.Category))
            {
                news.Category = "general";
            }

            news.Category = news.Category.ToLower();

            _context.News.Add(news);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetNews),
                new { id = news.Id },
                news);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(
            int id,
            News news)
        {
            if (id != news.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(news.Category))
            {
                news.Category = "general";
            }

            news.Category = news.Category.ToLower();

            _context.Entry(news).State =
                EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}