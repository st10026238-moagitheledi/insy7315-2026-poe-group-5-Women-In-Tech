using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "StaffMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Management", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Management", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Teachers", "/images/staff/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "ImageUrl" },
                values: new object[] { "Support Staff", "/images/staff/placeholder.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "StaffMembers");

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
    }
}
