using CommunityToolkit.Mvvm.ComponentModel;


namespace KoosMatliApp.Features.Teacher.Models
{
    public class LearnerItem : ObservableObject
    {
        public string FullName { get; set; } = string.Empty;

        public string StudentNumber { get; set; } = string.Empty;

        private string _attendanceStatus = "Not Marked";

        public string AttendanceStatus
        {
            get => _attendanceStatus;
            set => SetProperty(ref _attendanceStatus, value);
        }
    }
}
