using KoosMatliWebsiteAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly WebsiteDbContext _context;

        public HomeController(WebsiteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetHome()
        {
            return Ok(new
            {
                message = "Welcome to the Koos Matli Primary School Website API",
                school = "Koos Matli Primary School",
                motto = "Thuto ke Lesedi"
            });
        }
    }
}
