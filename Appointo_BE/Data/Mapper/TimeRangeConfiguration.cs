using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    public class TimeRangeConfiguration : IEntityTypeConfiguration<TimeRange>
    {
        public void Configure(EntityTypeBuilder<TimeRange> builder)
        {
            builder.HasKey(tr => tr.Id);

            builder.HasOne(tr => tr.StartTime).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(tr => tr.EndTime).WithMany().IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}