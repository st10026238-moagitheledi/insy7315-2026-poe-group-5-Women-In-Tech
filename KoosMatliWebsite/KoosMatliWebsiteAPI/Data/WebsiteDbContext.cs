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


            // =====================================================
            // GALLERY ITEMS
            // =====================================================

            modelBuilder.Entity<GalleryItem>().HasData(

                // =========================
                // ACADEMICS
                // =========================

                new GalleryItem
                {
                    Id = 1,
                    Title = "Learner Tutor",
                    Description = "A learner receiving academic support from a fellow learner tutor.",
                    ImageUrl = "/images/gallery/academics/tutor.jpg",
                    DateAdded = new DateTime(2026, 1, 15),
                    Category = "academics",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 2,
                    Title = "Teacher and Learners",
                    Description = "A teacher engaging with learners during an academic activity.",
                    ImageUrl = "/images/gallery/academics/teacher.jpg",
                    DateAdded = new DateTime(2026, 2, 10),
                    Category = "academics",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 3,
                    Title = "Parents",
                    Description = "Parents and guardians participating in the school community.",
                    ImageUrl = "/images/gallery/academics/parents.jpg",
                    DateAdded = new DateTime(2026, 2, 20),
                    Category = "academics",
                    IsPublished = true
                },


                // =========================
                // SPORTS
                // =========================

                new GalleryItem
                {
                    Id = 4,
                    Title = "Ziplining",
                    Description = "Learners enjoying an exciting ziplining activity.",
                    ImageUrl = "/images/gallery/sports/cable.jpg",
                    DateAdded = new DateTime(2026, 3, 5),
                    Category = "sports",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 5,
                    Title = "Athletics",
                    Description = "Learners taking part in an athletics activity.",
                    ImageUrl = "/images/gallery/sports/athletics.jpg",
                    DateAdded = new DateTime(2026, 3, 15),
                    Category = "sports",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 6,
                    Title = "Swimming Activity",
                    Description = "A learner participating in a school swimming activity.",
                    ImageUrl = "/images/gallery/sports/swim.jpg",
                    DateAdded = new DateTime(2026, 3, 25),
                    Category = "sports",
                    IsPublished = true
                },


                // =========================
                // EVENTS
                // =========================

                new GalleryItem
                {
                    Id = 7,
                    Title = "Breast Cancer Awareness",
                    Description = "The school community participating in a breast cancer awareness initiative.",
                    ImageUrl = "/images/gallery/events/cancer.jpg",
                    DateAdded = new DateTime(2026, 4, 10),
                    Category = "events",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 8,
                    Title = "Fun Day",
                    Description = "Learners and members of the school community enjoying a fun day.",
                    ImageUrl = "/images/gallery/events/fun.jpg",
                    DateAdded = new DateTime(2026, 5, 10),
                    Category = "events",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 9,
                    Title = "Market Day",
                    Description = "Learners participating in the school's Market Day activities.",
                    ImageUrl = "/images/gallery/events/market.jpg",
                    DateAdded = new DateTime(2026, 5, 20),
                    Category = "events",
                    IsPublished = true
                },


                // =========================
                // CULTURE
                // =========================

                new GalleryItem
                {
                    Id = 10,
                    Title = "Heritage Day",
                    Description = "Learners celebrating South African heritage and cultural diversity.",
                    ImageUrl = "/images/gallery/culture/heritage.jpg",
                    DateAdded = new DateTime(2026, 9, 24),
                    Category = "culture",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 11,
                    Title = "Literacy Day",
                    Description = "Learners taking part in activities promoting literacy and reading.",
                    ImageUrl = "/images/gallery/culture/literacy.jpg",
                    DateAdded = new DateTime(2026, 7, 15),
                    Category = "culture",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 12,
                    Title = "Careers Day",
                    Description = "Learners exploring different career opportunities and professions.",
                    ImageUrl = "/images/gallery/culture/careers.jpg",
                    DateAdded = new DateTime(2026, 8, 15),
                    Category = "culture",
                    IsPublished = true
                },


                // =========================
                // ACHIEVEMENTS
                // =========================

                new GalleryItem
                {
                    Id = 13,
                    Title = "Top Achievers",
                    Description = "Celebrating learners who achieved outstanding academic results.",
                    ImageUrl = "/images/gallery/achievements/top.jpg",
                    DateAdded = new DateTime(2026, 6, 20),
                    Category = "achievements",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 14,
                    Title = "Top Student",
                    Description = "Recognising a learner for outstanding academic achievement.",
                    ImageUrl = "/images/gallery/achievements/achiever.jpg",
                    DateAdded = new DateTime(2026, 6, 20),
                    Category = "achievements",
                    IsPublished = true
                },

                new GalleryItem
                {
                    Id = 15,
                    Title = "Principal Receives Certificate",
                    Description = "The school principal receiving a certificate in recognition of the school's achievements.",
                    ImageUrl = "/images/gallery/achievements/principal.jpg",
                    DateAdded = new DateTime(2026, 6, 25),
                    Category = "achievements",
                    IsPublished = true
                }
            );


            // =====================================================
            // SCHOOL EVENTS - OCTOBER 2026
            // =====================================================

            modelBuilder.Entity<SchoolEvent>().HasData(

                // =========================
                // REVISION
                // =========================

                new SchoolEvent
                {
                    Id = 1,
                    Title = "Term 4 Revision Classes Begin",
                    Description = "Revision classes begin to help learners prepare for the upcoming end-of-year examinations.",
                    Date = new DateTime(2026, 10, 5),
                    Category = "Academic",
                    Time = "14:00 - 15:30",
                    Location = "Koos Matli Primary School",
                    IsPublished = true
                },


                // =========================
                // PARENT MEETING
                // =========================

                new SchoolEvent
                {
                    Id = 2,
                    Title = "Parent Information Meeting",
                    Description = "Parents and guardians are invited to discuss learner progress, Term 4 revision support and examination preparation.",
                    Date = new DateTime(2026, 10, 7),
                    Category = "Meeting",
                    Time = "17:00 - 18:00",
                    Location = "School Hall",
                    IsPublished = true
                },


                // =========================
                // FUN DAY
                // =========================

                new SchoolEvent
                {
                    Id = 3,
                    Title = "School Fun Day",
                    Description = "A fun-filled school day with games, activities and entertainment for learners and staff.",
                    Date = new DateTime(2026, 10, 9),
                    Category = "Event",
                    Time = "09:00 - 14:00",
                    Location = "School Grounds",
                    IsPublished = true
                },


                // =========================
                // REVISION WEEK
                // =========================

                new SchoolEvent
                {
                    Id = 4,
                    Title = "Term 4 Revision Week",
                    Description = "Focused revision sessions continue across subjects as learners prepare for the end-of-year examinations.",
                    Date = new DateTime(2026, 10, 19),
                    Category = "Academic",
                    Time = "14:00 - 15:30",
                    Location = "Classrooms",
                    IsPublished = true
                },


                // =========================
                // EXAMINATIONS
                // =========================

                new SchoolEvent
                {
                    Id = 5,
                    Title = "Term 4 Examinations Begin",
                    Description = "End-of-year examinations commence. Learners are encouraged to arrive on time and bring all required examination materials.",
                    Date = new DateTime(2026, 10, 26),
                    Category = "Examination",
                    Time = "08:00",
                    Location = "Classrooms",
                    IsPublished = true
                }
            );
        }
    }
}