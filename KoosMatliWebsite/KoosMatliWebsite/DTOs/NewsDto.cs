namespace KoosMatliWebsite.DTOs
{
    public class NewsDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public DateTime PublishedDate { get; set; }

        public bool IsPublished { get; set; }

        public string Category { get; set; } = "general";
    }
}
