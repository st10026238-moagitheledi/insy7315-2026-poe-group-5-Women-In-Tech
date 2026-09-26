using Microsoft.AspNetCore.Mvc;
using KoosMatliWebsite.Models;

namespace KoosMatliWebsite.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            var staff = new List<StaffMember>

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

            return View(staff);
        }
    }
}
