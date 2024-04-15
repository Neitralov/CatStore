﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240415103822_SeedAdminDataAccount")]
    partial class SeedAdminDataAccount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Data.CartItem", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CatId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CatId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Domain.Data.Cat", b =>
                {
                    b.Property<Guid>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Cost")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric");

                    b.Property<string>("EarColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EyeColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsMale")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SkinColor")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CatId");

                    b.ToTable("Cats");
                });

            modelBuilder.Entity("Domain.Data.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Data.OrderItem", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CatId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("numeric");

                    b.HasKey("OrderId", "CatId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("Domain.Data.RefreshTokenSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SessionId"));

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("SessionId");

                    b.ToTable("RefreshTokenSessions");
                });

            modelBuilder.Entity("Domain.Data.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("CanEditCats")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("4f121d9c-1916-47d6-9bf8-7e394d0a9757"),
                            CanEditCats = true,
                            DateCreated = new DateTime(2024, 4, 15, 10, 38, 22, 96, DateTimeKind.Utc).AddTicks(5436),
                            Email = "admin@gmail.com",
                            PasswordHash = new byte[] { 230, 22, 252, 121, 246, 127, 221, 19, 41, 179, 15, 28, 104, 89, 153, 111, 156, 150, 230, 245, 55, 197, 54, 115, 204, 213, 235, 108, 145, 96, 111, 15, 88, 239, 9, 149, 95, 130, 125, 208, 138, 250, 125, 175, 3, 216, 170, 143, 39, 155, 175, 171, 93, 145, 203, 242, 182, 23, 48, 163, 101, 225, 253, 98 },
                            PasswordSalt = new byte[] { 152, 53, 239, 107, 91, 203, 127, 184, 35, 155, 17, 2, 193, 72, 34, 27, 150, 3, 247, 175, 75, 91, 228, 99, 77, 90, 85, 184, 26, 177, 195, 63, 223, 200, 61, 35, 7, 172, 110, 184, 105, 129, 202, 24, 75, 55, 216, 42, 221, 124, 135, 28, 83, 255, 63, 8, 236, 135, 66, 71, 44, 181, 31, 58, 175, 81, 246, 135, 65, 249, 130, 175, 54, 231, 89, 134, 14, 143, 229, 204, 200, 127, 136, 213, 50, 16, 69, 171, 79, 171, 56, 30, 227, 207, 83, 47, 170, 135, 109, 186, 182, 37, 231, 63, 90, 5, 176, 216, 193, 136, 128, 38, 181, 222, 251, 82, 247, 96, 168, 52, 72, 36, 28, 179, 12, 15, 5, 191 }
                        });
                });

            modelBuilder.Entity("Domain.Data.OrderItem", b =>
                {
                    b.HasOne("Domain.Data.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Data.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}