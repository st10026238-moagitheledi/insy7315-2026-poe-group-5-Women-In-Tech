namespace KoosMatliAdminApi.DTOs
{
    public class AdminDashboardDto
    {
        public int TotalLearners { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalParents { get; set; }

        public int NewApplications { get; set; }
        public int PendingApplications { get; set; }
        public int AnnouncementsSent { get; set; }
    }
}