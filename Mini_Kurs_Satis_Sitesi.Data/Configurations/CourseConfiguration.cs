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
            builder.HasKey(x=>x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        }
    }
}
