using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mini_Kurs_Satis_Sitesi.Core.Models;

namespace Mini_Kurs_Satis_Sitesi.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);  // Makul bir uzunluk sınırı

            builder.Property(x => x.PaymentDate)
                .IsRequired();

            builder.Property(x => x.TransactionId)
                .IsRequired()
                .HasMaxLength(100);  // Transaction ID'ler genelde uzun olabilir

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20);  // Success, Failed, Pending gibi durumlar için yeterli

            // Relationships - Order ile olan one-to-one ilişkisi
            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 