using Google.Cloud.Firestore;
using KoosMatliWebsiteAPI.Models;
using KoosMatliWebsiteAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KoosMatliWebsiteAPI.Controllers
{
    [Route("api/gallery")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public GalleryController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }


        // =====================================================
        // GET ALL GALLERY ITEMS
        // GET: api/gallery
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetGallery()
        {
            var database = _firebaseService.Database;

            var snapshot = await database
                .Collection("gallery")
                .OrderByDescending("DateAdded")
                .GetSnapshotAsync();

            var galleryItems = snapshot.Documents
                .Select(MapGalleryItem)
                .ToList();

            return Ok(galleryItems);
        }


        // =====================================================
        // GET GALLERY ITEM BY ID
        // GET: api/gallery/{id}
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGalleryItem(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("gallery")
                .Document(id.ToString());

            var snapshot = await documentReference
                .GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Gallery item with ID {id} was not found.");
            }

            var galleryItem = MapGalleryItem(snapshot);

            return Ok(galleryItem);
        }


        // =====================================================
        // CREATE GALLERY ITEM
        // POST: api/gallery
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateGalleryItem(
            GalleryItem galleryItem)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("gallery")
                .Document(galleryItem.Id.ToString());

            var existingSnapshot =
                await documentReference.GetSnapshotAsync();

            if (existingSnapshot.Exists)
            {
                return Conflict(
                    $"A gallery item with ID {galleryItem.Id} already exists.");
            }

            var data = GalleryItemToDictionary(galleryItem);

            await documentReference.SetAsync(data);

            return CreatedAtAction(
                nameof(GetGalleryItem),
                new { id = galleryItem.Id },
                galleryItem);
        }


        // =====================================================
        // UPDATE GALLERY ITEM
        // PUT: api/gallery/{id}
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGalleryItem(
            int id,
            GalleryItem updatedGalleryItem)
        {
            if (id != updatedGalleryItem.Id)
            {
                return BadRequest(
                    "The ID in the URL does not match the ID in the request.");
            }

            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("gallery")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Gallery item with ID {id} was not found.");
            }

            var data = GalleryItemToDictionary(
                updatedGalleryItem);

            await documentReference.SetAsync(
                data,
                SetOptions.Overwrite);

            return Ok(new
            {
                message = "Gallery item updated successfully.",
                galleryItem = updatedGalleryItem
            });
        }


        // =====================================================
        // DELETE GALLERY ITEM
        // DELETE: api/gallery/{id}
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGalleryItem(int id)
        {
            var database = _firebaseService.Database;

            var documentReference = database
                .Collection("gallery")
                .Document(id.ToString());

            var snapshot =
                await documentReference.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound(
                    $"Gallery item with ID {id} was not found.");
            }

            await documentReference.DeleteAsync();

            return Ok(new
            {
                message = "Gallery item deleted successfully."
            });
        }


        // =====================================================
        // FIRESTORE → GALLERY ITEM MODEL
        // =====================================================

        private static GalleryItem MapGalleryItem(
            DocumentSnapshot document)
        {
            return new GalleryItem
            {
                Id = GetInt(
                    document,
                    "Id"),

                Title = GetString(
                    document,
                    "Title"),

                Description = GetString(
                    document,
                    "Description"),

                ImageUrl = GetString(
                    document,
                    "ImageUrl"),

                DateAdded = GetDateTime(
                    document,
                    "DateAdded"),

                Category = GetString(
                    document,
                    "Category"),

                IsPublished = GetBool(
                    document,
                    "IsPublished")
            };
        }


        // =====================================================
        // GALLERY ITEM MODEL → FIRESTORE
        // =====================================================

        private static Dictionary<string, object> GalleryItemToDictionary(
            GalleryItem galleryItem)
        {
            return new Dictionary<string, object>
            {
                { "Id", galleryItem.Id },

                { "Title", galleryItem.Title },

                { "Description", galleryItem.Description },

                { "ImageUrl", galleryItem.ImageUrl },

                {
                    "DateAdded",
                    Timestamp.FromDateTime(
                        galleryItem.DateAdded.ToUniversalTime())
                },

                { "Category", galleryItem.Category },

                { "IsPublished", galleryItem.IsPublished }
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