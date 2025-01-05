using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Kurs_Satis_Sitesi.Core.Models;

namespace Mini_Kurs_Satis_Sitesi.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            // Relationships
            builder.HasOne(oi => oi.Course)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(oi => oi.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 