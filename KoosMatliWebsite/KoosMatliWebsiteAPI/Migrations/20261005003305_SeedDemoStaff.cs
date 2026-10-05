using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KoosMatliWebsiteAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "StaffMembers",
                columns: new[] { "Id", "Biography", "Department", "Email", "FullName", "ImageUrl", "IsActive", "Position" },
                values: new object[,]
                {
                    { 1, "Ms. Mokoena is dedicated to creating a supportive learning environment where every learner is encouraged to reach their full potential.", "School Management", "principal@koosmatliprimary.co.za", "Ms. Thandi Mokoena", "/images/staff/placeholder.png", true, "Principal" },
                    { 2, "Mr. Mthembu supports the school's academic and administrative programmes and works closely with staff, learners and parents.", "School Management", "k.mthembu@koosmatliprimary.co.za", "Mr. Kabelo Mthembu", "/images/staff/placeholder.png", true, "Deputy Principal" },
                    { 3, "Mrs. Nkosi is passionate about helping learners develop strong academic foundations and confidence in their abilities.", "Intermediate & Senior Phase", "l.nkosi@koosmatliprimary.co.za", "Mrs. Lerato Nkosi", "/images/staff/placeholder.png", true, "Grade 7 Teacher" },
                    { 4, "Mr. Dlamini encourages active learning and aims to make the classroom a positive and engaging environment for all learners.", "Intermediate Phase", "s.dlamini@koosmatliprimary.co.za", "Mr. Sipho Dlamini", "/images/staff/placeholder.png", true, "Grade 6 Teacher" },
                    { 5, "Ms. Molefe is committed to supporting learners academically while encouraging creativity, curiosity and responsible citizenship.", "Intermediate Phase", "n.molefe@koosmatliprimary.co.za", "Ms. Naledi Molefe", "/images/staff/placeholder.png", true, "Grade 5 Teacher" },
                    { 6, "Mrs. Mokoena focuses on creating a nurturing classroom environment that helps learners develop confidence and independence.", "Intermediate Phase", "p.mokoena@koosmatliprimary.co.za", "Mrs. Precious Mokoena", "/images/staff/placeholder.png", true, "Grade 4 Teacher" },
                    { 7, "Mr. Seema enjoys helping young learners build essential literacy, numeracy and social skills through engaging classroom activities.", "Foundation Phase", "t.seema@koosmatliprimary.co.za", "Mr. Tumelo Seema", "/images/staff/placeholder.png", true, "Grade 3 Teacher" },
                    { 8, "Ms. Ndlovu is passionate about early childhood education and creating positive learning experiences for young learners.", "Foundation Phase", "p.ndlovu@koosmatliprimary.co.za", "Ms. Palesa Ndlovu", "/images/staff/placeholder.png", true, "Grade 2 Teacher" },
                    { 9, "Mrs. Mahlangu helps young learners develop confidence, communication skills and a strong foundation for their educational journey.", "Foundation Phase", "a.mahlangu@koosmatliprimary.co.za", "Mrs. Amanda Mahlangu", "/images/staff/placeholder.png", true, "Grade 1 Teacher" },
                    { 10, "Ms. Maseko assists with the school's administrative activities and provides support to learners, parents and staff.", "Administration", "admin@koosmatliprimary.co.za", "Ms. Refilwe Maseko", "/images/staff/placeholder.png", true, "Administrative Officer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "StaffMembers",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
