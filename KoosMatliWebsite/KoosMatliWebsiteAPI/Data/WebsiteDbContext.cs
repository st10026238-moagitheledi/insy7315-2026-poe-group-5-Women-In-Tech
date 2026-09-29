using KoosMatliWebsiteAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Data
{
    public class WebsiteDbContext : DbContext
    {
        public WebsiteDbContext(DbContextOptions<WebsiteDbContext> options)
            : base(options)
        {
        }

        public DbSet<StaffMember> StaffMembers { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<SchoolEvent> SchoolEvents { get; set; }

        public DbSet<GalleryItem> GalleryItems { get; set; }

        public DbSet<Admission> Admissions { get; set; }

        public DbSet<SchoolInformation> SchoolInformation { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
