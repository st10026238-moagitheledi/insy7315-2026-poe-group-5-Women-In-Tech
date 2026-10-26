namespace KoosMatliAdminApi.Models
{
    public class SchoolClass
    {
        public int Id { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty;

        public string TeacherName { get; set; } = string.Empty;

        public int LearnerCount { get; set; }
    }
}
