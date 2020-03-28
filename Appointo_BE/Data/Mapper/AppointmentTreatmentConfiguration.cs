using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    internal class AppointmentTreatmentConfiguration : IEntityTypeConfiguration<AppointmentTreatment>
    {
        public void Configure(EntityTypeBuilder<AppointmentTreatment> builder)
        {
            builder.ToTable(nameof(AppointmentTreatment));
            builder.HasKey(at => new { at.AppointmentId, at.TreatmentId });

            builder.HasOne(at => at.Appointment)
                    .WithMany(a => a.Treatments)
                    .IsRequired()
                    .HasForeignKey(at => at.AppointmentId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(at => at.Treatment)
                    .WithMany()
                    .IsRequired()
                    .HasForeignKey(at => at.TreatmentId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}