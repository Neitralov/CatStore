﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Domain.Data.CartItem", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CatId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "CatId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Domain.Data.Cat", b =>
                {
                    b.Property<Guid>("CatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Cost")
                        .HasColumnType("TEXT");

                    b.Property<string>("EarColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EyeColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsMale")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SkinColor")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CatId");

                    b.ToTable("Cats");
                });

            modelBuilder.Entity("Domain.Data.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Domain.Data.OrderItem", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CatId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("OrderId", "CatId");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("Domain.Data.RefreshTokenSession", b =>
                {
                    b.Property<int>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId");

                    b.ToTable("RefreshTokenSessions");
                });

            modelBuilder.Entity("Domain.Data.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("CanEditCats")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("df45bfb9-96e9-437a-a41b-b6deb0b750bd"),
                            CanEditCats = true,
                            DateCreated = new DateTime(2024, 3, 19, 9, 28, 51, 60, DateTimeKind.Utc).AddTicks(6810),
                            Email = "admin@gmail.com",
                            PasswordHash = new byte[] { 211, 89, 233, 111, 28, 181, 70, 235, 35, 39, 239, 178, 43, 216, 7, 167, 183, 25, 70, 203, 54, 160, 100, 147, 38, 140, 42, 110, 33, 106, 64, 56, 52, 94, 40, 85, 13, 157, 160, 36, 151, 53, 106, 39, 138, 18, 112, 223, 250, 182, 67, 184, 47, 124, 84, 96, 205, 5, 50, 68, 40, 99, 123, 203 },
                            PasswordSalt = new byte[] { 16, 89, 114, 215, 130, 124, 164, 101, 143, 84, 110, 124, 17, 181, 168, 171, 34, 47, 230, 26, 40, 29, 214, 35, 229, 94, 60, 82, 140, 62, 164, 102, 88, 16, 128, 150, 209, 185, 117, 204, 144, 69, 30, 215, 85, 46, 37, 33, 88, 241, 138, 185, 159, 72, 127, 27, 76, 185, 214, 182, 27, 23, 145, 202, 59, 229, 216, 41, 5, 135, 111, 113, 168, 39, 189, 14, 250, 212, 234, 89, 144, 74, 105, 100, 233, 121, 55, 5, 165, 49, 37, 17, 46, 84, 135, 144, 164, 33, 11, 26, 179, 213, 155, 55, 204, 34, 22, 236, 248, 138, 93, 249, 186, 165, 71, 13, 114, 227, 217, 71, 210, 238, 151, 20, 128, 241, 81, 66 }
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
