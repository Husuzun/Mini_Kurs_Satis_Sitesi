using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Data.Configurations;

namespace Mini_Kurs_Satis_Sitesi.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserApp, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply configurations
            builder.ApplyConfiguration(new UserRefreshTokenConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new CourseConfiguration());
            builder.ApplyConfiguration(new UserAppConfiguration());

            // Configure decimal properties
            builder.Entity<Course>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            // Configure relationships
            builder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed Roles
            var instructorRoleId = "1";
            var userRoleId = "2";
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = instructorRoleId,
                    Name = "Instructor",
                    NormalizedName = "INSTRUCTOR"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // Password hasher
            var hasher = new PasswordHasher<UserApp>();

            // Seed First Instructor (Fatih Çakıroğlu)
            var instructor1Id = "1";
            var instructor1 = new UserApp
            {
                Id = instructor1Id,
                UserName = "fatih",
                NormalizedUserName = "FATIH",
                Email = "fatih@example.com",
                NormalizedEmail = "FATIH@EXAMPLE.COM",
                EmailConfirmed = true,
                City = "Istanbul",
                FirstName = "Fatih",
                LastName = "Çakıroğlu"
            };
            instructor1.PasswordHash = hasher.HashPassword(instructor1, "Password12*");

            // Seed Second Instructor (Ahmet Kaya)
            var instructor2Id = "2";
            var instructor2 = new UserApp
            {
                Id = instructor2Id,
                UserName = "ahmet",
                NormalizedUserName = "AHMET",
                Email = "ahmet@example.com",
                NormalizedEmail = "AHMET@EXAMPLE.COM",
                EmailConfirmed = true,
                City = "Istanbul",
                FirstName = "Ahmet",
                LastName = "Kaya"
            };
            instructor2.PasswordHash = hasher.HashPassword(instructor2, "Password12*");

            // Seed User (Hüseyin Uzun)
            var userId = "3";
            var user = new UserApp
            {
                Id = userId,
                UserName = "huseyin",
                NormalizedUserName = "HUSEYIN",
                Email = "huseyin@example.com",
                NormalizedEmail = "HUSEYIN@EXAMPLE.COM",
                EmailConfirmed = true,
                City = "Istanbul",
                FirstName = "Hüseyin",
                LastName = "Uzun"
            };
            user.PasswordHash = hasher.HashPassword(user, "Password12*");

            builder.Entity<UserApp>().HasData(instructor1, instructor2, user);

            // Assign Roles
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = instructorRoleId,
                    UserId = instructor1Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = instructorRoleId,
                    UserId = instructor2Id
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = userId
                }
            );

            // Seed Courses
            builder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Name = ".NET Core ile Mikroservis Mimarisi",
                    Description = "Mikroservis mimarisi ve .NET Core ile uygulama geliştirme",
                    Price = 199.99m,
                    Category = "Web Geliştirme",
                    InstructorId = instructor1Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Course
                {
                    Id = 2,
                    Name = "ASP.NET Core API Geliştirme",
                    Description = "RESTful API geliştirme ve best practices",
                    Price = 149.99m,
                    Category = "Web Geliştirme",
                    InstructorId = instructor1Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Course
                {
                    Id = 3,
                    Name = "Yapay Zeka ve Derin Öğrenme",
                    Description = "Python ile yapay zeka ve derin öğrenme uygulamaları",
                    Price = 299.99m,
                    Category = "Yapay Zeka",
                    InstructorId = instructor2Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                },
                new Course
                {
                    Id = 4,
                    Name = "Siber Güvenlik Temelleri",
                    Description = "Temel siber güvenlik konseptleri ve uygulamaları",
                    Price = 249.99m,
                    Category = "Siber Güvenlik",
                    InstructorId = instructor2Id,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                }
            );

            // Seed Order
            builder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = "Paid",
                    TotalPrice = 199.99m
                }
            );

            // Seed OrderItem
            builder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    CourseId = 1,
                    Price = 199.99m
                }
            );

            // Seed Payment
            builder.Entity<Payment>().HasData(
                new Payment
                {
                    Id = 1,
                    OrderId = 1,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "CreditCard",
                    Status = "Success",
                    Amount = 199.99m,
                    TransactionId = "tx_" + Guid.NewGuid().ToString()
                }
            );
        }
    }
}