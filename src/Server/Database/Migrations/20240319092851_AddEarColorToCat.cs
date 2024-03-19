using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEarColorToCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("60f1dbcc-e154-4af0-973a-dab7c9a41269"));

            migrationBuilder.AddColumn<string>(
                name: "EarColor",
                table: "Cats",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("df45bfb9-96e9-437a-a41b-b6deb0b750bd"), true, new DateTime(2024, 3, 19, 9, 28, 51, 60, DateTimeKind.Utc).AddTicks(6810), "admin@gmail.com", new byte[] { 211, 89, 233, 111, 28, 181, 70, 235, 35, 39, 239, 178, 43, 216, 7, 167, 183, 25, 70, 203, 54, 160, 100, 147, 38, 140, 42, 110, 33, 106, 64, 56, 52, 94, 40, 85, 13, 157, 160, 36, 151, 53, 106, 39, 138, 18, 112, 223, 250, 182, 67, 184, 47, 124, 84, 96, 205, 5, 50, 68, 40, 99, 123, 203 }, new byte[] { 16, 89, 114, 215, 130, 124, 164, 101, 143, 84, 110, 124, 17, 181, 168, 171, 34, 47, 230, 26, 40, 29, 214, 35, 229, 94, 60, 82, 140, 62, 164, 102, 88, 16, 128, 150, 209, 185, 117, 204, 144, 69, 30, 215, 85, 46, 37, 33, 88, 241, 138, 185, 159, 72, 127, 27, 76, 185, 214, 182, 27, 23, 145, 202, 59, 229, 216, 41, 5, 135, 111, 113, 168, 39, 189, 14, 250, 212, 234, 89, 144, 74, 105, 100, 233, 121, 55, 5, 165, 49, 37, 17, 46, 84, 135, 144, 164, 33, 11, 26, 179, 213, 155, 55, 204, 34, 22, 236, 248, 138, 93, 249, 186, 165, 71, 13, 114, 227, 217, 71, 210, 238, 151, 20, 128, 241, 81, 66 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("df45bfb9-96e9-437a-a41b-b6deb0b750bd"));

            migrationBuilder.DropColumn(
                name: "EarColor",
                table: "Cats");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("60f1dbcc-e154-4af0-973a-dab7c9a41269"), true, new DateTime(2024, 3, 18, 12, 54, 49, 680, DateTimeKind.Utc).AddTicks(262), "admin@gmail.com", new byte[] { 59, 101, 29, 162, 67, 118, 73, 172, 130, 28, 18, 197, 166, 231, 130, 213, 168, 52, 117, 234, 0, 62, 25, 102, 129, 103, 226, 223, 145, 247, 203, 235, 226, 30, 44, 0, 33, 19, 59, 215, 94, 169, 53, 173, 74, 103, 217, 103, 133, 46, 33, 65, 64, 58, 194, 105, 17, 55, 191, 190, 239, 78, 200, 172 }, new byte[] { 178, 141, 38, 106, 53, 65, 96, 152, 171, 15, 193, 135, 89, 170, 141, 39, 71, 57, 174, 22, 146, 165, 74, 126, 103, 107, 174, 93, 208, 222, 143, 206, 79, 200, 83, 56, 21, 68, 108, 94, 65, 134, 142, 141, 233, 21, 37, 16, 18, 169, 227, 65, 56, 149, 224, 51, 38, 242, 46, 91, 83, 145, 63, 34, 13, 220, 137, 246, 131, 54, 35, 167, 123, 78, 17, 176, 206, 250, 109, 13, 95, 72, 167, 52, 175, 7, 238, 94, 178, 5, 36, 21, 131, 53, 133, 146, 126, 125, 82, 14, 123, 216, 70, 139, 7, 13, 238, 84, 248, 66, 113, 111, 172, 215, 128, 122, 35, 41, 84, 246, 249, 243, 207, 50, 215, 172, 211, 105 } });
        }
    }
}
