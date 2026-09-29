namespace KoosMatliWebsiteAPI.Models
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public DateTime PublishedDate { get; set; }

        public bool IsPublished { get; set; } = true;

        public string Category { get; set; } = "general";
    }
}
