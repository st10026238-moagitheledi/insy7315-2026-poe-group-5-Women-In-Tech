namespace KoosMatliAdminApi.Models
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;

        public List<string> TeacherNames { get; set; } = new();
    }
}
