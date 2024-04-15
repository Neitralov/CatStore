using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminDataAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("4f121d9c-1916-47d6-9bf8-7e394d0a9757"), true, new DateTime(2024, 4, 15, 10, 38, 22, 96, DateTimeKind.Utc).AddTicks(5436), "admin@gmail.com", new byte[] { 230, 22, 252, 121, 246, 127, 221, 19, 41, 179, 15, 28, 104, 89, 153, 111, 156, 150, 230, 245, 55, 197, 54, 115, 204, 213, 235, 108, 145, 96, 111, 15, 88, 239, 9, 149, 95, 130, 125, 208, 138, 250, 125, 175, 3, 216, 170, 143, 39, 155, 175, 171, 93, 145, 203, 242, 182, 23, 48, 163, 101, 225, 253, 98 }, new byte[] { 152, 53, 239, 107, 91, 203, 127, 184, 35, 155, 17, 2, 193, 72, 34, 27, 150, 3, 247, 175, 75, 91, 228, 99, 77, 90, 85, 184, 26, 177, 195, 63, 223, 200, 61, 35, 7, 172, 110, 184, 105, 129, 202, 24, 75, 55, 216, 42, 221, 124, 135, 28, 83, 255, 63, 8, 236, 135, 66, 71, 44, 181, 31, 58, 175, 81, 246, 135, 65, 249, 130, 175, 54, 231, 89, 134, 14, 143, 229, 204, 200, 127, 136, 213, 50, 16, 69, 171, 79, 171, 56, 30, 227, 207, 83, 47, 170, 135, 109, 186, 182, 37, 231, 63, 90, 5, 176, 216, 193, 136, 128, 38, 181, 222, 251, 82, 247, 96, 168, 52, 72, 36, 28, 179, 12, 15, 5, 191 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("4f121d9c-1916-47d6-9bf8-7e394d0a9757"));
        }
    }
}
