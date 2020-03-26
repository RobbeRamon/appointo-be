using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    internal class TimeConfiguration : IEntityTypeConfiguration<Time>
    {
        public void Configure(EntityTypeBuilder<Time> builder)
        {
            builder.HasKey(t => t.Id);
        }
    }
}