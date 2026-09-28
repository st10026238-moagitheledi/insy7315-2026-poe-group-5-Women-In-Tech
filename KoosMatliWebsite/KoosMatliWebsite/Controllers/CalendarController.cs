using KoosMatliWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsite.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            var events = new List<SchoolEvent>
            {
                new SchoolEvent
                {
                    Title = "Term Begins",
                    Description = "First day of the new school term.",
                    Date = new DateTime(2026, 9, 1),
                    Category = "Academic",
                    Time = "07:30",
                    Location = "School Grounds"
                },

                new SchoolEvent
                {
                    Title = "Parents Meeting",
                    Description = "Parent and teacher meeting.",
                    Date = new DateTime(2026, 9, 10),
                    Category = "Meeting",
                    Time = "14:00",
                    Location = "School Hall"
                },

                new SchoolEvent
                {
                    Title = "Sports Day",
                    Description = "Annual school sports day.",
                    Date = new DateTime(2026, 9, 18),
                    Category = "Sports",
                    Time = "08:00",
                    Location = "School Sports Field"
                },

                new SchoolEvent
                {
                    Title = "Heritage Day",
                    Description = "Learners celebrate South African heritage.",
                    Date = new DateTime(2026, 9, 24),
                    Category = "School Event",
                    Time = "08:00",
                    Location = "School Grounds"
                },

                new SchoolEvent
                {
                    Title = "Term Assessments",
                    Description = "Learner assessments begin.",
                    Date = new DateTime(2026, 9, 28),
                    Category = "Academic",
                    Time = "08:00",
                    Location = "Classrooms"
                }
            };

            return View(events);
        }
    }
}
