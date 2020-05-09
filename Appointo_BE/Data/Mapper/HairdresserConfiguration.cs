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
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(hd => hd.Treatments)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(hd => hd.Email).IsRequired();
            builder.Property(hd => hd.Name).IsRequired();

            builder.HasOne(hd => hd.OpeningHours)
                    .WithMany();
        }
    }
}