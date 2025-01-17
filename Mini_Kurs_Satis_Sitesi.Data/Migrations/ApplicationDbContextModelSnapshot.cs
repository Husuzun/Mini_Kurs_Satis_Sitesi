﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mini_Kurs_Satis_Sitesi.Data;

#nullable disable

namespace Mini_Kurs_Satis_Sitesi.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            Name = "Instructor",
                            NormalizedName = "INSTRUCTOR"
                        },
                        new
                        {
                            Id = "2",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "1",
                            RoleId = "1"
                        },
                        new
                        {
                            UserId = "2",
                            RoleId = "1"
                        },
                        new
                        {
                            UserId = "3",
                            RoleId = "2"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("InstructorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InstructorId");

                    b.ToTable("Courses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Web Geliştirme",
                            CreatedDate = new DateTime(2025, 1, 5, 18, 28, 38, 700, DateTimeKind.Local).AddTicks(5442),
                            Description = "Mikroservis mimarisi ve .NET Core ile uygulama geliştirme",
                            InstructorId = "1",
                            IsActive = true,
                            Name = ".NET Core ile Mikroservis Mimarisi",
                            Price = 199.99m
                        },
                        new
                        {
                            Id = 2,
                            Category = "Web Geliştirme",
                            CreatedDate = new DateTime(2025, 1, 5, 18, 28, 38, 701, DateTimeKind.Local).AddTicks(6920),
                            Description = "RESTful API geliştirme ve best practices",
                            InstructorId = "1",
                            IsActive = true,
                            Name = "ASP.NET Core API Geliştirme",
                            Price = 149.99m
                        },
                        new
                        {
                            Id = 3,
                            Category = "Yapay Zeka",
                            CreatedDate = new DateTime(2025, 1, 5, 18, 28, 38, 701, DateTimeKind.Local).AddTicks(6934),
                            Description = "Python ile yapay zeka ve derin öğrenme uygulamaları",
                            InstructorId = "2",
                            IsActive = true,
                            Name = "Yapay Zeka ve Derin Öğrenme",
                            Price = 299.99m
                        },
                        new
                        {
                            Id = 4,
                            Category = "Siber Güvenlik",
                            CreatedDate = new DateTime(2025, 1, 5, 18, 28, 38, 701, DateTimeKind.Local).AddTicks(6944),
                            Description = "Temel siber güvenlik konseptleri ve uygulamaları",
                            InstructorId = "2",
                            IsActive = true,
                            Name = "Siber Güvenlik Temelleri",
                            Price = 249.99m
                        });
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            OrderDate = new DateTime(2025, 1, 5, 18, 28, 38, 701, DateTimeKind.Local).AddTicks(7991),
                            Status = "Paid",
                            TotalPrice = 199.99m,
                            UserId = "3"
                        });
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CourseId = 1,
                            OrderId = 1,
                            Price = 199.99m
                        });
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Payments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amount = 199.99m,
                            OrderId = 1,
                            PaymentDate = new DateTime(2025, 1, 5, 18, 28, 38, 701, DateTimeKind.Local).AddTicks(9817),
                            PaymentMethod = "CreditCard",
                            Status = "Success",
                            TransactionId = "tx_4bf913e6-2e21-4e98-be79-ddb285274238"
                        });
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1",
                            AccessFailedCount = 0,
                            City = "Istanbul",
                            ConcurrencyStamp = "deaa2219-ead6-4235-9cc4-512a093bbe2c",
                            Email = "fatih@example.com",
                            EmailConfirmed = true,
                            FirstName = "Fatih",
                            LastName = "Çakıroğlu",
                            LockoutEnabled = false,
                            NormalizedEmail = "FATIH@EXAMPLE.COM",
                            NormalizedUserName = "FATIH",
                            PasswordHash = "AQAAAAIAAYagAAAAEKK66wCN9xWgC/tSsLb0WpPePqNpOuS72nA9X2YAmL3jr/VSKFdud9ulzM82MolAwA==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "dfe2e8bd-f808-4d6b-8488-4058464665ac",
                            TwoFactorEnabled = false,
                            UserName = "fatih"
                        },
                        new
                        {
                            Id = "2",
                            AccessFailedCount = 0,
                            City = "Istanbul",
                            ConcurrencyStamp = "aee95d48-3615-40f5-8723-92ae35460066",
                            Email = "ahmet@example.com",
                            EmailConfirmed = true,
                            FirstName = "Ahmet",
                            LastName = "Kaya",
                            LockoutEnabled = false,
                            NormalizedEmail = "AHMET@EXAMPLE.COM",
                            NormalizedUserName = "AHMET",
                            PasswordHash = "AQAAAAIAAYagAAAAEA/pJv8zWI1mlal/4aEp0DNVvYQdtpk8LtVVsqTzd2RhZCPVtdksKzvTzzAOh3Zd+A==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "92a08e92-c357-4064-b89f-053875449b4e",
                            TwoFactorEnabled = false,
                            UserName = "ahmet"
                        },
                        new
                        {
                            Id = "3",
                            AccessFailedCount = 0,
                            City = "Istanbul",
                            ConcurrencyStamp = "5b1f17d4-c54f-4aa6-9720-7030a5359efb",
                            Email = "huseyin@example.com",
                            EmailConfirmed = true,
                            FirstName = "Hüseyin",
                            LastName = "Uzun",
                            LockoutEnabled = false,
                            NormalizedEmail = "HUSEYIN@EXAMPLE.COM",
                            NormalizedUserName = "HUSEYIN",
                            PasswordHash = "AQAAAAIAAYagAAAAEFbwprxAKM2XyW6AJoEdfGiv5ZcI1nZZYiuFhn7s/UbnyrsISnLBtXZjnCRZ4+urag==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "8b291335-0333-432d-b37c-987023d5a1ee",
                            TwoFactorEnabled = false,
                            UserName = "huseyin"
                        });
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.UserRefreshToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime>("Expiration")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId");

                    b.ToTable("UserRefreshTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Course", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", "Instructor")
                        .WithMany("Courses")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Order", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.OrderItem", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.Course", "Course")
                        .WithMany("OrderItems")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Payment", b =>
                {
                    b.HasOne("Mini_Kurs_Satis_Sitesi.Core.Models.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Mini_Kurs_Satis_Sitesi.Core.Models.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Course", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.Order", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("Payment")
                        .IsRequired();
                });

            modelBuilder.Entity("Mini_Kurs_Satis_Sitesi.Core.Models.UserApp", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
