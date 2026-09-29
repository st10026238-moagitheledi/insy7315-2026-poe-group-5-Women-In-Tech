namespace KoosMatliWebsiteAPI.Models
{
    public class StaffMember
    {
        public int Id { get; set; }

        public string FullName { get; set; } = "";

        public string Position { get; set; } = "";

        public string Department { get; set; } = "";

        public string Biography { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public string Email { get; set; } = "";

        public bool IsActive { get; set; } = true;
    }
}
