using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountToCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("df45bfb9-96e9-437a-a41b-b6deb0b750bd"));

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Cats",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("df39a4e8-1b48-4614-8480-a91467603fb8"), true, new DateTime(2024, 3, 19, 10, 8, 42, 158, DateTimeKind.Utc).AddTicks(24), "admin@gmail.com", new byte[] { 142, 196, 135, 58, 136, 89, 148, 251, 31, 9, 226, 87, 169, 149, 211, 2, 53, 109, 200, 90, 107, 56, 232, 65, 233, 254, 178, 66, 28, 66, 105, 192, 69, 121, 191, 248, 71, 255, 203, 45, 11, 64, 214, 64, 253, 173, 117, 182, 40, 37, 3, 22, 77, 216, 69, 126, 172, 245, 15, 1, 41, 84, 231, 0 }, new byte[] { 58, 43, 223, 88, 3, 221, 196, 55, 247, 235, 31, 215, 188, 112, 127, 132, 40, 76, 255, 26, 95, 210, 191, 251, 55, 253, 97, 63, 84, 194, 72, 80, 59, 53, 39, 228, 244, 180, 132, 58, 17, 116, 7, 93, 193, 167, 21, 94, 11, 171, 199, 186, 200, 123, 246, 77, 33, 70, 51, 21, 44, 171, 177, 205, 122, 57, 115, 3, 229, 4, 164, 135, 45, 120, 73, 165, 55, 36, 223, 133, 31, 119, 65, 119, 14, 168, 70, 84, 61, 19, 78, 140, 72, 68, 75, 211, 67, 61, 176, 156, 21, 220, 190, 153, 226, 133, 187, 20, 98, 13, 98, 155, 126, 18, 43, 28, 41, 27, 108, 21, 242, 86, 210, 35, 43, 173, 5, 7 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("df39a4e8-1b48-4614-8480-a91467603fb8"));

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Cats");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("df45bfb9-96e9-437a-a41b-b6deb0b750bd"), true, new DateTime(2024, 3, 19, 9, 28, 51, 60, DateTimeKind.Utc).AddTicks(6810), "admin@gmail.com", new byte[] { 211, 89, 233, 111, 28, 181, 70, 235, 35, 39, 239, 178, 43, 216, 7, 167, 183, 25, 70, 203, 54, 160, 100, 147, 38, 140, 42, 110, 33, 106, 64, 56, 52, 94, 40, 85, 13, 157, 160, 36, 151, 53, 106, 39, 138, 18, 112, 223, 250, 182, 67, 184, 47, 124, 84, 96, 205, 5, 50, 68, 40, 99, 123, 203 }, new byte[] { 16, 89, 114, 215, 130, 124, 164, 101, 143, 84, 110, 124, 17, 181, 168, 171, 34, 47, 230, 26, 40, 29, 214, 35, 229, 94, 60, 82, 140, 62, 164, 102, 88, 16, 128, 150, 209, 185, 117, 204, 144, 69, 30, 215, 85, 46, 37, 33, 88, 241, 138, 185, 159, 72, 127, 27, 76, 185, 214, 182, 27, 23, 145, 202, 59, 229, 216, 41, 5, 135, 111, 113, 168, 39, 189, 14, 250, 212, 234, 89, 144, 74, 105, 100, 233, 121, 55, 5, 165, 49, 37, 17, 46, 84, 135, 144, 164, 33, 11, 26, 179, 213, 155, 55, 204, 34, 22, 236, 248, 138, 93, 249, 186, 165, 71, 13, 114, 227, 217, 71, 210, 238, 151, 20, 128, 241, 81, 66 } });
        }
    }
}
