using System.ComponentModel.DataAnnotations;


namespace KoosMatliWebsite.Models
{
    public class AdmissionApplication
    {
        public int Id { get; set; }

        [Required]
        public string LearnerFirstName { get; set; } = string.Empty;

        [Required]
        public string LearnerLastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; } 

        [Required]
        public string GradeApplyingFor { get; set; } = string.Empty;

        [Required]
        public string ParentName { get; set;  } = string.Empty;

        [Required]
        [EmailAddress]
        public string ParentEmail { get; set;  } = string.Empty;

        [Required]
        [Phone]
        public string ParentPhone { get; set;  } = string.Empty;

        public string? PreviousSchool { get; set;  } = string.Empty;

        public DateTime AppicationDate { get; set; } = DateTime.Now;
        public string Status { get; set;  } = "Pending";
    }
}
