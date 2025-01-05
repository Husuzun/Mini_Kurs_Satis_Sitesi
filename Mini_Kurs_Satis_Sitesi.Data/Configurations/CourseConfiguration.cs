using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Kurs_Satis_Sitesi.Core.Models;

namespace Mini_Kurs_Satis_Sitesi.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Category)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ImageUrl)
                .HasMaxLength(500);  // URL'ler için makul bir uzunluk

            builder.Property(x => x.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.OrderItems)
                .WithOne(oi => oi.Course)
                .HasForeignKey(oi => oi.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
