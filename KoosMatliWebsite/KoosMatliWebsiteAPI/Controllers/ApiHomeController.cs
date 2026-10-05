using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
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