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
    [Migration("20240125111724_InititalCreate")]
    partial class InititalCreate
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

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("3aef1961-a007-4d85-b2a5-8ceec92dba0d"),
                            DateCreated = new DateTime(2024, 1, 25, 11, 17, 24, 572, DateTimeKind.Utc).AddTicks(71),
                            Email = "admin@gmail.com",
                            PasswordHash = new byte[] { 64, 14, 250, 7, 105, 227, 226, 19, 159, 69, 67, 194, 7, 248, 96, 233, 141, 146, 188, 222, 166, 103, 140, 166, 220, 89, 23, 151, 149, 43, 196, 91, 146, 67, 56, 7, 11, 254, 38, 175, 225, 146, 125, 246, 140, 98, 209, 87, 219, 179, 227, 38, 126, 212, 57, 239, 196, 10, 204, 240, 80, 84, 148, 95 },
                            PasswordSalt = new byte[] { 152, 103, 88, 217, 245, 187, 78, 167, 99, 69, 68, 91, 56, 18, 191, 23, 6, 237, 144, 108, 212, 255, 11, 241, 233, 119, 197, 31, 244, 254, 97, 191, 214, 185, 39, 145, 65, 121, 205, 25, 36, 7, 241, 39, 72, 250, 252, 111, 5, 79, 34, 195, 67, 132, 25, 175, 150, 55, 147, 14, 40, 134, 151, 37, 148, 65, 234, 86, 157, 188, 249, 153, 41, 255, 252, 209, 119, 157, 47, 233, 78, 158, 22, 60, 203, 112, 79, 45, 229, 253, 208, 109, 128, 178, 253, 197, 238, 238, 78, 89, 40, 98, 12, 158, 15, 209, 240, 5, 77, 171, 48, 38, 26, 26, 239, 193, 28, 211, 19, 193, 112, 37, 125, 130, 144, 148, 217, 134 },
                            Role = "admin"
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
