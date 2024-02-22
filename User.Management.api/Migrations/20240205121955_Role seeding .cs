using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace User.Management.api.Migrations
{
    /// <inheritdoc />
    public partial class Roleseeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "47102082-d0e2-4d56-baf2-54a34f0caeff", "2", "Hr", "HR" },
                    { "5bff6511-1b50-4ec9-8de3-6218b4f83e00", "3", "User", "USER" },
                    { "796cf9a3-5712-428d-b082-1a924fb49b3e", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47102082-d0e2-4d56-baf2-54a34f0caeff");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5bff6511-1b50-4ec9-8de3-6218b4f83e00");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "796cf9a3-5712-428d-b082-1a924fb49b3e");
        }
    }
}
