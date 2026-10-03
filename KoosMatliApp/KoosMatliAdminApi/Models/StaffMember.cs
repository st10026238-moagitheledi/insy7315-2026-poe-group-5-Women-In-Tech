namespace KoosMatliAdminApi.Models
{
    public class StaffMember
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public List<string> Subjects { get; set; } = new();

        public List<string> Classes { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
    }
}
