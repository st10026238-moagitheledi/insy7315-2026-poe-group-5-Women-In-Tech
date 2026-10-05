namespace KoosMatliWebsiteAPI.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string ParentFirstName { get; set; } = "";
        public string ParentLastName { get; set; } = "";
        public string ParentEmail { get; set; } = "";
        public string ParentPhone { get; set; } = "";
        public string RelationshipToLearner { get; set; } = "";
        public string LearnerFirstName { get; set; } = "";
        public string LearnerLastName { get; set; } = "";
        public DateTime LearnerDateOfBirth { get; set; }
        public string GradeApplyingFor { get; set; } = "";
        public string PreviousSchool { get; set; } = "";
        public string HomeAddress { get; set; } = "";
        public string AdditionalInformation { get; set; } = "";
        public string Status { get; set; } = "Pending";
        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow;
    }
}
