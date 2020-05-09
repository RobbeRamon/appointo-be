using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data
{
    public class OpeningHoursConfiguration : IEntityTypeConfiguration<OpeningHours>
    {
        public void Configure(EntityTypeBuilder<OpeningHours> builder)
        {
            builder.HasKey(oh => oh.Id);
            builder.HasMany(oh => oh.WorkDays)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}