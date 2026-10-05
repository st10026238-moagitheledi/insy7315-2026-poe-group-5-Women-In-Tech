using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewsArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "Id", "Category", "Content", "ImageUrl", "IsPublished", "PublishedDate", "Title" },
                values: new object[,]
                {
                    { 1, "announcements", "We warmly welcome all our learners, parents and guardians back to Koos Matli Primary School. We look forward to another successful term filled with learning, growth and exciting opportunities.", "/images/news/welcome-back.jpg", true, new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Welcome Back to Koos Matli Primary School" },
                    { 2, "announcements", "Parents and guardians are kindly reminded to ensure that learners arrive at school wearing the correct school uniform. A neat and complete uniform helps promote pride, discipline and a strong sense of belonging.", "/images/news/school-uniform.jpg", true, new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "School Uniform Reminder" },
                    { 3, "events", "Learners are invited to participate in our annual Sports Day. The day will include athletics, team activities and fun sporting events. Parents and guardians are encouraged to come and support our young athletes.", "/images/news/sports-day.jpg", true, new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual Sports Day" },
                    { 4, "events", "Parents and guardians are invited to attend our upcoming parents meeting. The meeting will provide an opportunity to discuss learner progress, school activities and important information for the term.", "/images/news/parents-meeting.jpg", true, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parents and Guardians Meeting" },
                    { 5, "events", "Our school community will come together for a special Cultural Celebration Day. Learners will have the opportunity to celebrate South Africa's diverse cultures through music, storytelling, performances and traditional activities.", "/images/news/cultural-day.jpg", true, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cultural Celebration Day" },
                    { 6, "achievements", "We are proud to celebrate our learners who represented Koos Matli Primary School in a recent academic competition. Their dedication, preparation and commitment to learning are an inspiration to the entire school community.", "/images/news/academic-achievement.jpg", true, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Our Learners Shine in Academic Competition" },
                    { 7, "achievements", "Congratulations to our reading champions who have demonstrated outstanding dedication to developing their reading skills. We encourage all learners to continue discovering the joy of reading and learning.", "/images/news/reading-champions.jpg", true, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Celebrating Our Reading Champions" },
                    { 8, "achievements", "We are delighted to recognise learners who have shown exceptional effort in academics, sport, leadership and positive behaviour. Their achievements demonstrate the values of commitment, perseverance and excellence.", "/images/news/learner-excellence.jpg", true, new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Celebrating Learner Excellence" },
                    { 9, "general", "At Koos Matli Primary School, we believe that every learner deserves a safe, welcoming and supportive environment. Our teachers and staff continue working together to create an environment where learners can learn, grow and reach their full potential.", "/images/news/learning-environment.jpg", true, new DateTime(2026, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Creating a Positive Learning Environment" },
                    { 10, "general", "Regular school attendance plays an important role in learner success. We encourage parents and guardians to support learners by ensuring they attend school consistently and arrive on time each day.", "/images/news/school-attendance.jpg", true, new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Importance of Regular Attendance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
