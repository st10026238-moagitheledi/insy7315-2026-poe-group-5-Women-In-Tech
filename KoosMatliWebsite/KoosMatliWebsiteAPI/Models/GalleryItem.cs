namespace KoosMatliWebsiteAPI.Models
{
    public class GalleryItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public DateTime DateAdded { get; set; }

        public string Category { get; set; } = "general";

        public bool IsPublished { get; set; } = true;
    }
}
