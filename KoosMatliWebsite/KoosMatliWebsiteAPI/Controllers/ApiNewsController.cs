using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public NewsController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        // =====================================================
        // GET ALL NEWS
        // GET: api/news
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var database = _firebaseService.Database;

            var snapshot = await database
                .Collection("news")
                .OrderByDescending("PublishedDate")
                .GetSnapshotAsync();

            var news = snapshot.Documents
                .Select(MapNews)
                .ToList();

            return Ok(news);
        }


        // =====================================================
        // GET NEWS BY ID
        // GET: api/news/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("news")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"News article with ID {id} was not found.");
            }

            var news = MapNews(snapshot);

            return Ok(news);
        }


        // =====================================================
        // CREATE NEWS
        // POST: api/news
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateNews(News news)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("news")
                .Document(news.Id.ToString());

            var existingSnapshot =
                await documentReference.GetSnapshotAsync();

            if (existingSnapshot.Exists)
            {
                return Conflict(
                    $"A news article with ID {news.Id} already exists.");
            }

            var data = NewsToDictionary(news);

            await documentReference.SetAsync(data);

            return CreatedAtAction(
                nameof(GetNewsById),
                new { id = news.Id },
                news);
        }


        // =====================================================
        // UPDATE NEWS
        // PUT: api/news/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateNews(
            int id,
            News updatedNews)
        {
            if (id != updatedNews.Id)
            {
                return BadRequest(
                    "The ID in the URL does not match the ID in the request.");
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("news")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"News article with ID {id} was not found.");
            }

            var data = NewsToDictionary(updatedNews);

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return Ok(new
            {
                message = "News article updated successfully.",
                news = updatedNews
            });
        }


        // =====================================================
        // DELETE NEWS
        // DELETE: api/news/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("news")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"News article with ID {id} was not found.");
            }

            await documentReference.DeleteAsync();

            return Ok(new
            {
                message = "News article deleted successfully."
            });
        }


        // =====================================================
        // FIRESTORE → NEWS MODEL
        // =====================================================

        private static News MapNews(
            DocumentSnapshot document)
        {
            return new News
            {
                Id = GetInt(document, "Id"),

                Title = GetString(
                    document,
                    "Title"),

                Content = GetString(
                    document,
                    "Content"),

                ImageUrl = GetString(
                    document,
                    "ImageUrl"),

                PublishedDate = GetDateTime(
                    document,
                    "PublishedDate"),

                IsPublished = GetBool(
                    document,
                    "IsPublished"),

                Category = GetString(
                    document,
                    "Category")
            };
        }


        // =====================================================
        // NEWS MODEL → FIRESTORE
        // =====================================================

        private static Dictionary<string, object> NewsToDictionary(
            News news)
        {
            return new Dictionary<string, object>
            {
                { "Id", news.Id },
                { "Title", news.Title },
                { "Content", news.Content },
                { "ImageUrl", news.ImageUrl },
                { "PublishedDate", Timestamp.FromDateTime(
                    news.PublishedDate.ToUniversalTime()) },
                { "IsPublished", news.IsPublished },
                { "Category", news.Category }
            };
        }

        // =====================================================
        // FIRESTORE FIELD HELPERS
        // =====================================================

        private static string GetString(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return "";
            }

            return document.GetValue<string>(field) ?? "";
        }


        private static int GetInt(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return 0;
            }

            return document.GetValue<int>(field);
        }


        private static bool GetBool(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return true;
            }

            return document.GetValue<bool>(field);
        }


        private static DateTime GetDateTime(
            DocumentSnapshot document,
            string field)
        {
            if (!document.ContainsField(field))
            {
                return DateTime.MinValue;
            }

            var timestamp =
                document.GetValue<Timestamp>(field);

            return timestamp.ToDateTime();
        }
    }
}