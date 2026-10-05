using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirebaseTestController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public FirebaseTestController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpGet]
        public IActionResult TestFirebase()
        {
            try
            {
                var database = _firebaseService.Database;

                return Ok(new
                {
                    success = true,
                    message = "Firebase Firestore connection is working."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Firebase connection failed.",
                    error = ex.Message
                });
            }
        }
    }
}
