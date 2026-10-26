namespace KoosMatliAdminApi.Models
{
    public class AdmissionApplication
    {
        public int Id { get; set; }

        public string ApplicationNumber { get; set; } = string.Empty;

        public string LearnerFirstName { get; set; } = string.Empty;
        public string LearnerLastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public string GradeApplyingFor { get; set; } = string.Empty;

        public string GuardianName { get; set; } = string.Empty;
        public string GuardianEmail { get; set; } = string.Empty;
        public string GuardianPhone { get; set; } = string.Empty;

        public DateTime DateSubmitted { get; set; }

        public string Status { get; set; } = "New";

        public string LearnerFullName => $"{LearnerFirstName} {LearnerLastName}";
    }
}
