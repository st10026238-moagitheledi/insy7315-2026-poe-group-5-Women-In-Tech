namespace KoosMatliWebsiteAPI.Models
{
    public class GalleryItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public DateTime DateAdded { get; set; }

        public bool IsPublished { get; set; } = true;
    }
}
