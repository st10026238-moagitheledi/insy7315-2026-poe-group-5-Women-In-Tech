namespace KoosMatliWebsite.DTOs
{
    public class SchoolEventDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime Date { get; set; }

        public string Category { get; set; } = "";

        public string Time { get; set; } = "";

        public string Location { get; set; } = "";

        public bool IsPublished { get; set; }
    }
}
