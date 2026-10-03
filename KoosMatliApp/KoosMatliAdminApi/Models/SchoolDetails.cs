namespace KoosMatliAdminApi.Models
{
    public class SchoolDetails
    {
        public int Id { get; set; }

        public string SchoolName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PrincipalName { get; set; } = string.Empty;
    }
}
