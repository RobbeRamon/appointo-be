using Appointo_BE.Data.Mapper;
using Appointo_BE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Appointo_BE.Data
{
    public class AppointoDbContext : IdentityDbContext
    {
        public DbSet<Hairdresser> Hairdressers { get; set; }

        public AppointoDbContext(DbContextOptions<AppointoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new HairdresserConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new WorkDayConfiguration());
            modelBuilder.ApplyConfiguration(new TreatmentConfiguration());
            modelBuilder.ApplyConfiguration(new TimeConfiguration());
            modelBuilder.ApplyConfiguration(new OpeningHoursConfiguration());
            modelBuilder.ApplyConfiguration(new AppointmentTreatmentConfiguration());
            modelBuilder.ApplyConfiguration(new TimeRangeConfiguration());
        }
    }
}
