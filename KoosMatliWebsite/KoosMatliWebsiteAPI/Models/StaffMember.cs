namespace KoosMatliWebsiteAPI.Models
{
    public class StaffMember
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Position { get; set; } = "";

        public string Category { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public string Qualification { get; set; } = "";

        public string Bio { get; set; } = "";

        public bool IsActive { get; set; } = true;
    }
}
