using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelationshipToLearner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearnerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearnerLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearnerDateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GradeApplyingFor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousSchool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
