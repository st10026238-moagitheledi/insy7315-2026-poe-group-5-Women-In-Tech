using Microsoft.AspNetCore.Mvc;
using KoosMatliWebsite.Models;

namespace KoosMatliWebsite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class NewsController : ControllerBase
    {
        private static readonly List<SchoolEvent> NewsItems = new()
        {
            new SchoolEvent
            {
                Title = "Grade 5 Learners Excel in District Quiz",
                Description = "Our Grade 5 team showed excellent knowledge and teamwork at the district quiz competition.",
                Category = "achievement",
                Date = new DateTime(2024, 5, 12),
                Time = "10:00",
                Location = "District Hall",
                ImageUrl = "https://images.unsplash.com/photo-1509062522246-3755977927d7?auto=format&fit=crop&w=700&q=80",
                ImageAlt = "Learners participating in an academic activity"
            },

            new SchoolEvent
            {
                Title = "School Sports Day a Huge Success!",
                Description = "Thank you to all our learners, parents and staff for making Sports Day a memorable event.",
                Category = "event",
                Date = new DateTime(2024, 5, 5),
                Time = "14:00",
                Location = "School Grounds",
                ImageUrl = "https://images.unsplash.com/photo-1461896836934-ffe607ba8211?auto=format&fit=crop&w=700&q=80",
                ImageAlt = "Learners participating in sports"
            },

            new SchoolEvent
            {
                Title = "Parent-Teacher Meetings",
                Description = "Parent-Teacher meetings will be held on 20 May 2024. More details to follow.",
                Category = "announcement",
                Date = new DateTime(2024, 4, 30),
                Time = "",
                Location = "",
                ImageUrl = "https://images.unsplash.com/photo-1546410531-bb4caa6b424d?auto=format&fit=crop&w=700&q=80",
                ImageAlt = "School meeting"
            },

            new SchoolEvent
            {
                Title = "Welcome Back to Koos Matli",
                Description = "We look forward to another exciting term of learning, growth and community.",
                Category = "general",
                Date = new DateTime(2024, 4, 22),
                Time = "",
                Location = "",
                ImageUrl = "https://images.unsplash.com/photo-1580582932707-520aed937b7b?auto=format&fit=crop&w=700&q=80",
                ImageAlt = "School building"
            }
        };

        // GET: api/news
        [HttpGet]
        public IActionResult GetAllNews()
        {
            return Ok(NewsItems);
        }

        // GET: api/news/title/Grade%205%20Learners%20Excel%20in%20District%20Quiz
        [HttpGet("title/{title}")]
        public IActionResult GetNewsByTitle(string title)
        {
            var news = NewsItems.FirstOrDefault(n =>
                n.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (news == null)
            {
                return NotFound(new
                {
                    message = "News item not found."
                });
            }

            return Ok(news);
        }

        // GET: api/news/category/event
        [HttpGet("category/{category}")]
        public IActionResult GetNewsByCategory(string category)
        {
            var news = NewsItems
                .Where(n => n.Category.Equals(
                    category,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(news);
        }
    }
}