using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class InititalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => new { x.UserId, x.CatId });
                });

            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    CatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SkinColor = table.Column<string>(type: "TEXT", nullable: false),
                    EyeColor = table.Column<string>(type: "TEXT", nullable: false),
                    IsMale = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.CatId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CatId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => new { x.OrderId, x.CatId });
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DateCreated", "Email", "PasswordHash", "PasswordSalt", "Role" },
                values: new object[] { new Guid("3aef1961-a007-4d85-b2a5-8ceec92dba0d"), new DateTime(2024, 1, 25, 11, 17, 24, 572, DateTimeKind.Utc).AddTicks(71), "admin@gmail.com", new byte[] { 64, 14, 250, 7, 105, 227, 226, 19, 159, 69, 67, 194, 7, 248, 96, 233, 141, 146, 188, 222, 166, 103, 140, 166, 220, 89, 23, 151, 149, 43, 196, 91, 146, 67, 56, 7, 11, 254, 38, 175, 225, 146, 125, 246, 140, 98, 209, 87, 219, 179, 227, 38, 126, 212, 57, 239, 196, 10, 204, 240, 80, 84, 148, 95 }, new byte[] { 152, 103, 88, 217, 245, 187, 78, 167, 99, 69, 68, 91, 56, 18, 191, 23, 6, 237, 144, 108, 212, 255, 11, 241, 233, 119, 197, 31, 244, 254, 97, 191, 214, 185, 39, 145, 65, 121, 205, 25, 36, 7, 241, 39, 72, 250, 252, 111, 5, 79, 34, 195, 67, 132, 25, 175, 150, 55, 147, 14, 40, 134, 151, 37, 148, 65, 234, 86, 157, 188, 249, 153, 41, 255, 252, 209, 119, 157, 47, 233, 78, 158, 22, 60, 203, 112, 79, 45, 229, 253, 208, 109, 128, 178, 253, 197, 238, 238, 78, 89, 40, 98, 12, 158, 15, 209, 240, 5, 77, 171, 48, 38, 26, 26, 239, 193, 28, 211, 19, 193, 112, 37, 125, 130, 144, 148, 217, 134 }, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Cats");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
