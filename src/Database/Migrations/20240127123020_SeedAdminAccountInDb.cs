using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminAccountInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("8fe644cb-5ac7-421d-966b-4f6c66f4b0e1"), true, new DateTime(2024, 1, 27, 12, 30, 20, 293, DateTimeKind.Utc).AddTicks(7413), "admin@gmail.com", new byte[] { 161, 222, 117, 138, 43, 221, 163, 204, 148, 96, 199, 173, 80, 151, 127, 150, 236, 22, 59, 113, 66, 153, 199, 98, 133, 205, 225, 200, 4, 33, 52, 12, 7, 42, 186, 43, 163, 252, 236, 89, 186, 244, 161, 239, 174, 78, 198, 219, 237, 125, 17, 51, 25, 145, 231, 241, 217, 111, 25, 170, 237, 19, 1, 220 }, new byte[] { 204, 113, 53, 173, 57, 253, 71, 49, 169, 212, 57, 1, 19, 62, 31, 25, 159, 211, 32, 233, 129, 241, 26, 91, 11, 128, 115, 36, 36, 214, 158, 79, 238, 69, 47, 15, 116, 200, 112, 242, 50, 214, 188, 18, 94, 85, 218, 40, 222, 57, 223, 72, 100, 69, 224, 22, 68, 227, 225, 171, 92, 25, 232, 245, 167, 200, 250, 249, 94, 47, 174, 27, 51, 237, 240, 78, 149, 239, 215, 216, 113, 38, 223, 24, 221, 204, 95, 177, 171, 52, 110, 87, 154, 31, 123, 40, 143, 252, 9, 177, 48, 74, 19, 35, 57, 124, 90, 225, 132, 94, 83, 251, 193, 21, 192, 21, 152, 16, 11, 132, 167, 209, 202, 99, 74, 246, 248, 115 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("8fe644cb-5ac7-421d-966b-4f6c66f4b0e1"));
        }
    }
}
