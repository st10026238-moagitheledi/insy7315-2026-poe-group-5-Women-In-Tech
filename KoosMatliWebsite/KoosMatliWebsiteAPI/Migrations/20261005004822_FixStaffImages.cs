using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixStaffImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/staff/placeholder.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/staff/placeholder.jpg");
        }
    }
}
