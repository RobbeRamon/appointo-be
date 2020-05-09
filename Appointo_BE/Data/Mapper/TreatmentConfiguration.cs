using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointo_BE.Data.Mapper
{
    public class TreatmentConfiguration : IEntityTypeConfiguration<Treatment>
    {
        public void Configure(EntityTypeBuilder<Treatment> builder)
        {
            builder.HasKey(tr => tr.Id);

            builder.Property(tr => tr.Name)
                    .IsRequired();

            builder.Property(tr => tr.Duration)
                    .IsRequired();

            builder.Property(tr => tr.Price)
                .IsRequired();

            builder.Property(tr => tr.Category)
                    .IsRequired();
        }
    }
}