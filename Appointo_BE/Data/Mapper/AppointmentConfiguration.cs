using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.HasMany(a => a.Treatments)
                    .WithOne(tr => tr.Appointment)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.Property(a => a.StartMoment);
        }
    }
}