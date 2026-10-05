using System.ComponentModel.DataAnnotations;

namespace KoosMatliWebsite.DTOs
{
    public class ApplicationDto
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Parent first name is required.")]
        [Display(Name = "First Name")]
        public string ParentFirstName { get; set; } = "";

        [Required(ErrorMessage = "Parent last name is required.")]
        [Display(Name = "Last Name")]
        public string ParentLastName { get; set; } = "";

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string ParentEmail { get; set; } = "";

        [Required(ErrorMessage = "Phone number is required.")]
        [Display(Name = "Phone Number")]
        public string ParentPhone { get; set; } = "";

        [Required(ErrorMessage = "Please select your relationship to the learner.")]
        [Display(Name = "Relationship to Learner")]
        public string RelationshipToLearner { get; set; } = "";


        [Required(ErrorMessage = "Learner first name is required.")]
        [Display(Name = "First Name")]
        public string LearnerFirstName { get; set; } = "";

        [Required(ErrorMessage = "Learner last name is required.")]
        [Display(Name = "Last Name")]
        public string LearnerLastName { get; set; } = "";

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? LearnerDateOfBirth { get; set; }

        [Required(ErrorMessage = "Please select the grade.")]
        [Display(Name = "Grade Applying For")]
        public string GradeApplyingFor { get; set; } = "";

        [Display(Name = "Previous School")]
        public string PreviousSchool { get; set; } = "";


        [Required(ErrorMessage = "Home address is required.")]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; } = "";
        [Display(Name = "Additional Information")]

        public string AdditionalInformation { get; set; } = "";

        public string Status { get; set; } = "Pending";

        public DateTime DateSubmitted { get; set; }
    }
}