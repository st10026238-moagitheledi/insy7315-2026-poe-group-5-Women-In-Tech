using Microsoft.AspNetCore.Mvc;
using KoosMatliAdminApi.DTOs;

namespace KoosMatliAdminApi.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    public class AdminDashboardController : ControllerBase
    {
        [HttpGet]
        public ActionResult<AdminDashboardDto> GetDashboard()
        {
            var dashboard = new AdminDashboardDto
            {
                TotalLearners = 520,
                TotalTeachers = 28,
                TotalParents = 410,

                NewApplications = 12,
                PendingApplications = 8,
                AnnouncementsSent = 3
            };

            return Ok(dashboard);
        }
    }
}

