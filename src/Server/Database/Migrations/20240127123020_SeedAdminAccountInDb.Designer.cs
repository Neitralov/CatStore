﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240127123020_SeedAdminAccountInDb")]
    partial class SeedAdminAccountInDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("OrderId", "CatId");

                    b.ToTable("OrderItem");
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
                            UserId = new Guid("8fe644cb-5ac7-421d-966b-4f6c66f4b0e1"),
                            CanEditCats = true,
                            DateCreated = new DateTime(2024, 1, 27, 12, 30, 20, 293, DateTimeKind.Utc).AddTicks(7413),
                            Email = "admin@gmail.com",
                            PasswordHash = new byte[] { 161, 222, 117, 138, 43, 221, 163, 204, 148, 96, 199, 173, 80, 151, 127, 150, 236, 22, 59, 113, 66, 153, 199, 98, 133, 205, 225, 200, 4, 33, 52, 12, 7, 42, 186, 43, 163, 252, 236, 89, 186, 244, 161, 239, 174, 78, 198, 219, 237, 125, 17, 51, 25, 145, 231, 241, 217, 111, 25, 170, 237, 19, 1, 220 },
                            PasswordSalt = new byte[] { 204, 113, 53, 173, 57, 253, 71, 49, 169, 212, 57, 1, 19, 62, 31, 25, 159, 211, 32, 233, 129, 241, 26, 91, 11, 128, 115, 36, 36, 214, 158, 79, 238, 69, 47, 15, 116, 200, 112, 242, 50, 214, 188, 18, 94, 85, 218, 40, 222, 57, 223, 72, 100, 69, 224, 22, 68, 227, 225, 171, 92, 25, 232, 245, 167, 200, 250, 249, 94, 47, 174, 27, 51, 237, 240, 78, 149, 239, 215, 216, 113, 38, 223, 24, 221, 204, 95, 177, 171, 52, 110, 87, 154, 31, 123, 40, 143, 252, 9, 177, 48, 74, 19, 35, 57, 124, 90, 225, 132, 94, 83, 251, 193, 21, 192, 21, 152, 16, 11, 132, 167, 209, 202, 99, 74, 246, 248, 115 }
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