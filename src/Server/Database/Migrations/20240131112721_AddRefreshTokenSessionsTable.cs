using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenSessionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("8fe644cb-5ac7-421d-966b-4f6c66f4b0e1"));

            migrationBuilder.CreateTable(
                name: "RefreshTokenSessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenSessions", x => x.SessionId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("c2b63ae5-fa37-48a8-969e-464e732146f5"), true, new DateTime(2024, 1, 31, 11, 27, 21, 181, DateTimeKind.Utc).AddTicks(4618), "admin@gmail.com", new byte[] { 196, 196, 202, 74, 120, 79, 96, 198, 246, 87, 29, 224, 99, 62, 171, 231, 152, 0, 59, 87, 234, 168, 180, 185, 140, 51, 244, 35, 105, 194, 154, 66, 246, 133, 109, 243, 200, 81, 172, 187, 91, 220, 80, 139, 166, 156, 88, 73, 230, 190, 33, 91, 27, 95, 238, 228, 94, 139, 159, 226, 1, 209, 248, 16 }, new byte[] { 67, 21, 120, 107, 102, 245, 107, 154, 17, 134, 18, 53, 79, 145, 245, 141, 129, 70, 209, 71, 160, 61, 203, 46, 61, 211, 133, 47, 139, 143, 251, 240, 52, 11, 88, 116, 112, 171, 30, 32, 236, 231, 32, 56, 72, 187, 64, 70, 192, 173, 129, 153, 61, 238, 37, 176, 164, 67, 188, 12, 146, 209, 44, 143, 26, 35, 1, 216, 132, 166, 89, 76, 246, 248, 190, 179, 121, 120, 117, 54, 210, 169, 140, 110, 85, 231, 157, 1, 88, 92, 29, 155, 14, 215, 45, 9, 94, 35, 254, 175, 45, 72, 39, 147, 243, 149, 89, 201, 218, 75, 83, 226, 192, 81, 248, 192, 150, 186, 146, 138, 157, 54, 20, 166, 43, 41, 71, 59 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokenSessions");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("c2b63ae5-fa37-48a8-969e-464e732146f5"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CanEditCats", "DateCreated", "Email", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("8fe644cb-5ac7-421d-966b-4f6c66f4b0e1"), true, new DateTime(2024, 1, 27, 12, 30, 20, 293, DateTimeKind.Utc).AddTicks(7413), "admin@gmail.com", new byte[] { 161, 222, 117, 138, 43, 221, 163, 204, 148, 96, 199, 173, 80, 151, 127, 150, 236, 22, 59, 113, 66, 153, 199, 98, 133, 205, 225, 200, 4, 33, 52, 12, 7, 42, 186, 43, 163, 252, 236, 89, 186, 244, 161, 239, 174, 78, 198, 219, 237, 125, 17, 51, 25, 145, 231, 241, 217, 111, 25, 170, 237, 19, 1, 220 }, new byte[] { 204, 113, 53, 173, 57, 253, 71, 49, 169, 212, 57, 1, 19, 62, 31, 25, 159, 211, 32, 233, 129, 241, 26, 91, 11, 128, 115, 36, 36, 214, 158, 79, 238, 69, 47, 15, 116, 200, 112, 242, 50, 214, 188, 18, 94, 85, 218, 40, 222, 57, 223, 72, 100, 69, 224, 22, 68, 227, 225, 171, 92, 25, 232, 245, 167, 200, 250, 249, 94, 47, 174, 27, 51, 237, 240, 78, 149, 239, 215, 216, 113, 38, 223, 24, 221, 204, 95, 177, 171, 52, 110, 87, 154, 31, 123, 40, 143, 252, 9, 177, 48, 74, 19, 35, 57, 124, 90, 225, 132, 94, 83, 251, 193, 21, 192, 21, 152, 16, 11, 132, 167, 209, 202, 99, 74, 246, 248, 115 } });
        }
    }
}
