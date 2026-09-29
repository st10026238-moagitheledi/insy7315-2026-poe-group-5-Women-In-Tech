namespace KoosMatliWebsiteAPI.Models
{
    public class Admission
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string Requirements { get; set; } = "";

        public string ApplicationInstructions { get; set; } = "";

        public string ApplicationLink { get; set; } = "";

        public bool IsActive { get; set; } = true;
    }
}
