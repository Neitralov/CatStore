using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddNamePropertyToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c2b63ae5-fa37-48a8-969e-464e732146f5"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderItem",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("60f1dbcc-e154-4af0-973a-dab7c9a41269"), true, new DateTime(2024, 3, 18, 12, 54, 49, 680, DateTimeKind.Utc).AddTicks(262), "admin@gmail.com", new byte[] { 59, 101, 29, 162, 67, 118, 73, 172, 130, 28, 18, 197, 166, 231, 130, 213, 168, 52, 117, 234, 0, 62, 25, 102, 129, 103, 226, 223, 145, 247, 203, 235, 226, 30, 44, 0, 33, 19, 59, 215, 94, 169, 53, 173, 74, 103, 217, 103, 133, 46, 33, 65, 64, 58, 194, 105, 17, 55, 191, 190, 239, 78, 200, 172 }, new byte[] { 178, 141, 38, 106, 53, 65, 96, 152, 171, 15, 193, 135, 89, 170, 141, 39, 71, 57, 174, 22, 146, 165, 74, 126, 103, 107, 174, 93, 208, 222, 143, 206, 79, 200, 83, 56, 21, 68, 108, 94, 65, 134, 142, 141, 233, 21, 37, 16, 18, 169, 227, 65, 56, 149, 224, 51, 38, 242, 46, 91, 83, 145, 63, 34, 13, 220, 137, 246, 131, 54, 35, 167, 123, 78, 17, 176, 206, 250, 109, 13, 95, 72, 167, 52, 175, 7, 238, 94, 178, 5, 36, 21, 131, 53, 133, 146, 126, 125, 82, 14, 123, 216, 70, 139, 7, 13, 238, 84, 248, 66, 113, 111, 172, 215, 128, 122, 35, 41, 84, 246, 249, 243, 207, 50, 215, 172, 211, 105 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("60f1dbcc-e154-4af0-973a-dab7c9a41269"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderItem");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("c2b63ae5-fa37-48a8-969e-464e732146f5"), true, new DateTime(2024, 1, 31, 11, 27, 21, 181, DateTimeKind.Utc).AddTicks(4618), "admin@gmail.com", new byte[] { 196, 196, 202, 74, 120, 79, 96, 198, 246, 87, 29, 224, 99, 62, 171, 231, 152, 0, 59, 87, 234, 168, 180, 185, 140, 51, 244, 35, 105, 194, 154, 66, 246, 133, 109, 243, 200, 81, 172, 187, 91, 220, 80, 139, 166, 156, 88, 73, 230, 190, 33, 91, 27, 95, 238, 228, 94, 139, 159, 226, 1, 209, 248, 16 }, new byte[] { 67, 21, 120, 107, 102, 245, 107, 154, 17, 134, 18, 53, 79, 145, 245, 141, 129, 70, 209, 71, 160, 61, 203, 46, 61, 211, 133, 47, 139, 143, 251, 240, 52, 11, 88, 116, 112, 171, 30, 32, 236, 231, 32, 56, 72, 187, 64, 70, 192, 173, 129, 153, 61, 238, 37, 176, 164, 67, 188, 12, 146, 209, 44, 143, 26, 35, 1, 216, 132, 166, 89, 76, 246, 248, 190, 179, 121, 120, 117, 54, 210, 169, 140, 110, 85, 231, 157, 1, 88, 92, 29, 155, 14, 215, 45, 9, 94, 35, 254, 175, 45, 72, 39, 147, 243, 149, 89, 201, 218, 75, 83, 226, 192, 81, 248, 192, 150, 186, 146, 138, 157, 54, 20, 166, 43, 41, 71, 59 } });
        }
    }
}
