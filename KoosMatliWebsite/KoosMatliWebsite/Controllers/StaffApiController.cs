using KoosMatliWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    [ApiController]
    [Route("api/staff")]
    public class StaffApiController : Controller
    {
        private static readonly List<StaffMember> StaffMembers = new()
        {
            new StaffMember
            {
                Name = "Mr. J. Matli",
                Role = "Principal",
                Category = "Management",
                ImageUrl = "https://randomuser.me/api/portraits/men/32.jpg"
            },

            new StaffMember
            {
                Name = "Mrs. L. Mokoena",
                Role = "Deputy Principal",
                Category = "Management",
                ImageUrl = "https://randomuser.me/api/portraits/women/44.jpg"
            },

            new StaffMember
            {
                Name = "Mr. S. Dlamini",
                Role = "HOD: Foundation Phase",
                Category = "Management",
                ImageUrl = "https://randomuser.me/api/portraits/men/45.jpg"
            },

            new StaffMember
            {
                Name = "Mrs. P. Ndlovu",
                Role = "HOD: Intermediate Phase",
                Category = "Management",
                ImageUrl = "https://randomuser.me/api/portraits/women/65.jpg"
            },

            new StaffMember
            {
                Name = "Mrs. K. Sibaya",
                Role = "LO Teacher",
                Category = "Teachers",
                ImageUrl = "https://randomuser.me/api/portraits/women/26.jpg"
            },

            new StaffMember
            {
                Name = "Mr. T. Maseto",
                Role = "Life Orientation Teacher",
                Category = "Teachers",
                ImageUrl = "https://randomuser.me/api/portraits/men/52.jpg"
            },

            new StaffMember
            {
                Name = "Mrs. A. Jacobs",
                Role = "Grade 4 Teacher",
                Category = "Teachers",
                ImageUrl = "https://randomuser.me/api/portraits/women/49.jpg"
            },

            new StaffMember
            {
                Name = "Mr. B. Mahlangu",
                Role = "Sports Coordinator",
                Category = "Support Staff",
                ImageUrl = "https://randomuser.me/api/portraits/men/67.jpg"
            }
        };

        // GET: api/staff
        [HttpGet]
        public IActionResult GetAllStaff()
        {
            return Ok(StaffMembers);
        }

        // GET: api/staff/category/Teachers
        [HttpGet("category/{category}")]
        public IActionResult GetStaffByCategory(string category)
        {
            var staff = StaffMembers
                .Where(s => s.Category.Equals(
                    category,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(staff);
        }

        // GET: api/staff/name/Mr.%20J.%20Matli
        [HttpGet("name/{name}")]
        public IActionResult GetStaffByName(string name)
        {
            var staff = StaffMembers.FirstOrDefault(s =>
                s.Name.Equals(
                    name,
                    StringComparison.OrdinalIgnoreCase));

            if (staff == null)
            {
                return NotFound(new
                {
                    message = "Staff member not found."
                });
            }

            return Ok(staff);
        }
    }
}