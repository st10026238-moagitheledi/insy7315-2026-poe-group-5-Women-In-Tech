using KoosMatliWebsiteAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KoosMatliWebsiteAPI.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
            : base(options)
        {
        }

        public DbSet<StaffMember> StaffMembers { get; set; }
    }
}
