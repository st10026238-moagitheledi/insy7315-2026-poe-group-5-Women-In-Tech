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

        public DbSet<Application> Applications { get; set; }

        public DbSet<SchoolInformation> SchoolInformation { get; set; }

        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StaffMember>().HasData(
                // =========================
                // MANAGEMENT
                // =========================

                new StaffMember
                {
                    Id = 1,
                    FullName = "Ms. Thandi Mokoena",
                    Position = "Principal",
                    Department = "School Management",
                    Category = "Management",
                    Biography = "Ms. Mokoena is dedicated to creating a supportive learning environment where every learner is encouraged to reach their full potential.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "principal@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 2,
                    FullName = "Mr. Kabelo Mthembu",
                    Position = "Deputy Principal",
                    Department = "School Management",
                    Category = "Management",
                    Biography = "Mr. Mthembu supports the school's academic and administrative programmes and works closely with staff, learners and parents.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "k.mthembu@koosmatliprimary.co.za",
                    IsActive = true
                },


                // =========================
                // TEACHERS
                // =========================

                new StaffMember
                {
                    Id = 3,
                    FullName = "Mrs. Lerato Nkosi",
                    Position = "Grade 7 Teacher",
                    Department = "Intermediate & Senior Phase",
                    Category = "Teachers",
                    Biography = "Mrs. Nkosi is passionate about helping learners develop strong academic foundations and confidence in their abilities.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "l.nkosi@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 4,
                    FullName = "Mr. Sipho Dlamini",
                    Position = "Grade 6 Teacher",
                    Department = "Intermediate Phase",
                    Category = "Teachers",
                    Biography = "Mr. Dlamini encourages active learning and aims to make the classroom a positive and engaging environment for all learners.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "s.dlamini@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 5,
                    FullName = "Ms. Naledi Molefe",
                    Position = "Grade 5 Teacher",
                    Department = "Intermediate Phase",
                    Category = "Teachers",
                    Biography = "Ms. Molefe is committed to supporting learners academically while encouraging creativity, curiosity and responsible citizenship.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "n.molefe@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 6,
                    FullName = "Mrs. Precious Mokoena",
                    Position = "Grade 4 Teacher",
                    Department = "Intermediate Phase",
                    Category = "Teachers",
                    Biography = "Mrs. Mokoena focuses on creating a nurturing classroom environment that helps learners develop confidence and independence.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "p.mokoena@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 7,
                    FullName = "Mr. Tumelo Seema",
                    Position = "Grade 3 Teacher",
                    Department = "Foundation Phase",
                    Category = "Teachers",
                    Biography = "Mr. Seema enjoys helping young learners build essential literacy, numeracy and social skills through engaging classroom activities.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "t.seema@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 8,
                    FullName = "Ms. Palesa Ndlovu",
                    Position = "Grade 2 Teacher",
                    Department = "Foundation Phase",
                    Category = "Teachers",
                    Biography = "Ms. Ndlovu is passionate about early childhood education and creating positive learning experiences for young learners.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "p.ndlovu@koosmatliprimary.co.za",
                    IsActive = true
                },

                new StaffMember
                {
                    Id = 9,
                    FullName = "Mrs. Amanda Mahlangu",
                    Position = "Grade 1 Teacher",
                    Department = "Foundation Phase",
                    Category = "Teachers",
                    Biography = "Mrs. Mahlangu helps young learners develop confidence, communication skills and a strong foundation for their educational journey.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "a.mahlangu@koosmatliprimary.co.za",
                    IsActive = true
                },


                // =========================
                // SUPPORT STAFF
                // =========================

                new StaffMember
                {
                    Id = 10,
                    FullName = "Ms. Refilwe Maseko",
                    Position = "Administrative Officer",
                    Department = "Administration",
                    Category = "Support Staff",
                    Biography = "Ms. Maseko assists with the school's administrative activities and provides support to learners, parents and staff.",
                    ImageUrl = "/images/staff/placeholder.png",
                    Email = "admin@koosmatliprimary.co.za",
                    IsActive = true
                }
            );
        }
    }
}