using Appointo_BE.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Data
{
    public class AppointoDbContext : DbContext
    {
        public DbSet<Hairdresser> Hairdressers { get; set; }

        public AppointoDbContext(DbContextOptions<AppointoDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        
    }
}
