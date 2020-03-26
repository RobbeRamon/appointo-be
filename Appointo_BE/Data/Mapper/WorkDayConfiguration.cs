using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    public class WorkDayConfiguration : IEntityTypeConfiguration<WorkDay>
    {
        public void Configure(EntityTypeBuilder<WorkDay> builder)
        {
            builder.HasKey(wd => wd.Id);
        }
    }
}