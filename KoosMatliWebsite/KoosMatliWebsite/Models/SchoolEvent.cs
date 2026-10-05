namespace KoosMatliWebsite.Models
{
    public class SchoolEvent
    {
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public string Category { get; set; } = "";

        public string Time { get; set; } = "";

        public string Location { get; set; } = "";
        public string ImageUrl { get; set; } = "";

        public string ImageAlt { get; set; } = "";
    }
}
