using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Appointo_BE.Data.Mapper
{
    public class WorkDayConfiguration : IEntityTypeConfiguration<WorkDay>
    {
        public void Configure(EntityTypeBuilder<WorkDay> builder)
        {
            builder.HasKey(wd => wd.Id);
            builder.HasMany(wd => wd.Hours).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(wd => wd.Day).IsRequired();
        }
    }
}