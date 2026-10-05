using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOctober2026SchoolEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SchoolEvents",
                columns: new[] { "Id", "Category", "Date", "Description", "IsPublished", "Location", "Time", "Title" },
                values: new object[,]
                {
                    { 1, "Academic", new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Revision classes begin to help learners prepare for the upcoming end-of-year examinations.", true, "Koos Matli Primary School", "14:00 - 15:30", "Term 4 Revision Classes Begin" },
                    { 2, "Meeting", new DateTime(2026, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parents and guardians are invited to discuss learner progress, Term 4 revision support and examination preparation.", true, "School Hall", "17:00 - 18:00", "Parent Information Meeting" },
                    { 3, "Event", new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "A fun-filled school day with games, activities and entertainment for learners and staff.", true, "School Grounds", "09:00 - 14:00", "School Fun Day" },
                    { 4, "Academic", new DateTime(2026, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Focused revision sessions continue across subjects as learners prepare for the end-of-year examinations.", true, "Classrooms", "14:00 - 15:30", "Term 4 Revision Week" },
                    { 5, "Examination", new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "End-of-year examinations commence. Learners are encouraged to arrive on time and bring all required examination materials.", true, "Classrooms", "08:00", "Term 4 Examinations Begin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SchoolEvents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SchoolEvents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SchoolEvents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SchoolEvents",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SchoolEvents",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
