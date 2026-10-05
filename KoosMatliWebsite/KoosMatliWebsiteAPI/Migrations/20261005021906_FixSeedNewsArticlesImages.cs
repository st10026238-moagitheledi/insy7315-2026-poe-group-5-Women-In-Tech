using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedNewsArticlesImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/news/placeholder.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/news/welcome-back.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/news/school-uniform.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/news/sports-day.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/news/parents-meeting.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/news/cultural-day.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/news/academic-achievement.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/news/reading-champions.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "/images/news/learner-excellence.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/news/learning-environment.jpg");

            migrationBuilder.UpdateData(
                table: "News",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/news/school-attendance.jpg");
        }
    }
}
