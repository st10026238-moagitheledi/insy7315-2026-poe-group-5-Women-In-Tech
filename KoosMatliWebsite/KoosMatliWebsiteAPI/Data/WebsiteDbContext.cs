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


            // =====================================================
            // STAFF MEMBERS
            // =====================================================

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


            // =====================================================
            // NEWS ARTICLES
            // =====================================================

            modelBuilder.Entity<News>().HasData(

                // =========================
                // ANNOUNCEMENTS
                // =========================

                new News
                {
                    Id = 1,
                    Title = "Welcome Back to Koos Matli Primary School",
                    Content = "We warmly welcome all our learners, parents and guardians back to Koos Matli Primary School. We look forward to another successful term filled with learning, growth and exciting opportunities.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 1, 15),
                    IsPublished = true,
                    Category = "announcements"
                },

                new News
                {
                    Id = 2,
                    Title = "School Uniform Reminder",
                    Content = "Parents and guardians are kindly reminded to ensure that learners arrive at school wearing the correct school uniform. A neat and complete uniform helps promote pride, discipline and a strong sense of belonging.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 2, 10),
                    IsPublished = true,
                    Category = "announcements"
                },


                // =========================
                // EVENTS
                // =========================

                new News
                {
                    Id = 3,
                    Title = "Annual Sports Day",
                    Content = "Learners are invited to participate in our annual Sports Day. The day will include athletics, team activities and fun sporting events. Parents and guardians are encouraged to come and support our young athletes.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 2, 20),
                    IsPublished = true,
                    Category = "events"
                },

                new News
                {
                    Id = 4,
                    Title = "Parents and Guardians Meeting",
                    Content = "Parents and guardians are invited to attend our upcoming parents meeting. The meeting will provide an opportunity to discuss learner progress, school activities and important information for the term.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 3, 15),
                    IsPublished = true,
                    Category = "events"
                },

                new News
                {
                    Id = 5,
                    Title = "Cultural Celebration Day",
                    Content = "Our school community will come together for a special Cultural Celebration Day. Learners will have the opportunity to celebrate South Africa's diverse cultures through music, storytelling, performances and traditional activities.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 4, 10),
                    IsPublished = true,
                    Category = "events"
                },


                // =========================
                // ACHIEVEMENTS
                // =========================

                new News
                {
                    Id = 6,
                    Title = "Our Learners Shine in Academic Competition",
                    Content = "We are proud to celebrate our learners who represented Koos Matli Primary School in a recent academic competition. Their dedication, preparation and commitment to learning are an inspiration to the entire school community.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 2, 28),
                    IsPublished = true,
                    Category = "achievements"
                },

                new News
                {
                    Id = 7,
                    Title = "Celebrating Our Reading Champions",
                    Content = "Congratulations to our reading champions who have demonstrated outstanding dedication to developing their reading skills. We encourage all learners to continue discovering the joy of reading and learning.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 3, 25),
                    IsPublished = true,
                    Category = "achievements"
                },

                new News
                {
                    Id = 8,
                    Title = "Celebrating Learner Excellence",
                    Content = "We are delighted to recognise learners who have shown exceptional effort in academics, sport, leadership and positive behaviour. Their achievements demonstrate the values of commitment, perseverance and excellence.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 5, 20),
                    IsPublished = true,
                    Category = "achievements"
                },


                // =========================
                // GENERAL
                // =========================

                new News
                {
                    Id = 9,
                    Title = "Creating a Positive Learning Environment",
                    Content = "At Koos Matli Primary School, we believe that every learner deserves a safe, welcoming and supportive environment. Our teachers and staff continue working together to create an environment where learners can learn, grow and reach their full potential.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 4, 5),
                    IsPublished = true,
                    Category = "general"
                },

                new News
                {
                    Id = 10,
                    Title = "The Importance of Regular Attendance",
                    Content = "Regular school attendance plays an important role in learner success. We encourage parents and guardians to support learners by ensuring they attend school consistently and arrive on time each day.",
                    ImageUrl = "/images/news/placeholder.png",
                    PublishedDate = new DateTime(2026, 5, 5),
                    IsPublished = true,
                    Category = "general"
                }
            );
        }
    }
}