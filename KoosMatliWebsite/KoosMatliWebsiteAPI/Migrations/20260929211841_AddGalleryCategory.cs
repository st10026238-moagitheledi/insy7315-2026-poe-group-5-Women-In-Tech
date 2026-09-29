using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGalleryCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "GalleryItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "GalleryItems");
        }
    }
}
