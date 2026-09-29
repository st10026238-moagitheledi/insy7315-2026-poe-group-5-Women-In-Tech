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
                schoolName = "Koos Matli Primary School",
                tagline = "Where Education Lights the Way",
                message = "Welcome to Koos Matli Primary School."
            });
        }
    }
}
