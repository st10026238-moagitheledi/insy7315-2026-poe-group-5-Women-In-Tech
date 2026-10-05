using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedGalleryItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GalleryItems",
                columns: new[] { "Id", "Category", "DateAdded", "Description", "ImageUrl", "IsPublished", "Title" },
                values: new object[,]
                {
                    { 1, "academics", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "A learner receiving academic support from a fellow learner tutor.", "/images/gallery/academics/tutor.jpg", true, "Learner Tutor" },
                    { 2, "academics", new DateTime(2026, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "A teacher engaging with learners during an academic activity.", "/images/gallery/academics/teacher.jpg", true, "Teacher and Learners" },
                    { 3, "academics", new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Parents and guardians participating in the school community.", "/images/gallery/academics/parents.jpg", true, "Parents" },
                    { 4, "sports", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners enjoying an exciting ziplining activity.", "/images/gallery/sports/cable.jpg", true, "Ziplining" },
                    { 5, "sports", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners taking part in an athletics activity.", "/images/gallery/sports/athletics.jpg", true, "Athletics" },
                    { 6, "sports", new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "A learner participating in a school swimming activity.", "/images/gallery/sports/swim.jpg", true, "Swimming Activity" },
                    { 7, "events", new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "The school community participating in a breast cancer awareness initiative.", "/images/gallery/events/cancer.jpg", true, "Breast Cancer Awareness" },
                    { 8, "events", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners and members of the school community enjoying a fun day.", "/images/gallery/events/fun.jpg", true, "Fun Day" },
                    { 9, "events", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners participating in the school's Market Day activities.", "/images/gallery/events/market.jpg", true, "Market Day" },
                    { 10, "culture", new DateTime(2026, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners celebrating South African heritage and cultural diversity.", "/images/gallery/culture/heritage.jpg", true, "Heritage Day" },
                    { 11, "culture", new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners taking part in activities promoting literacy and reading.", "/images/gallery/culture/literacy.jpg", true, "Literacy Day" },
                    { 12, "culture", new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learners exploring different career opportunities and professions.", "/images/gallery/culture/careers.jpg", true, "Careers Day" },
                    { 13, "achievements", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Celebrating learners who achieved outstanding academic results.", "/images/gallery/achievements/top.jpg", true, "Top Achievers" },
                    { 14, "achievements", new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Recognising a learner for outstanding academic achievement.", "/images/gallery/achievements/achiever.jpg", true, "Top Student" },
                    { 15, "achievements", new DateTime(2026, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "The school principal receiving a certificate in recognition of the school's achievements.", "/images/gallery/achievements/principal.jpg", true, "Principal Receives Certificate" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "GalleryItems",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
