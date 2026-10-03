using KoosMatliAdminApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliAdminApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<AdmissionApplication> Applications { get; set; }

        public DbSet<StaffMember> StaffMembers { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<SchoolDetails> SchoolDetails { get; set; }

        public DbSet<SchoolDate> SchoolDates { get; set; }

        public DbSet<SchoolDocument> SchoolDocuments { get; set; }
    }
}