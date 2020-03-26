using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    public class HairdresserConfiguration : IEntityTypeConfiguration<Hairdresser>
    {
        public void Configure(EntityTypeBuilder<Hairdresser> builder)
        {
            builder.ToTable(nameof(Hairdresser));

            builder.HasKey(hd => hd.Id);
            builder.HasMany(hd => hd.Appointments)
                    .WithOne();
            builder.HasMany(hd => hd.Treatments)
                    .WithOne();
        }
    }
}