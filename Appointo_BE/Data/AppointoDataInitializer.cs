using Appointo_BE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Data
{
    public class AppointoDataInitializer
    {
        private readonly AppointoDbContext _dbContext;

        public AppointoDataInitializer(AppointoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            
            if(_dbContext.Database.EnsureCreated())
            {
                Hairdresser hairdresser1 = new Hairdresser { Name = "Hairlounge Marlies"};
                Hairdresser hairdresser2 = new Hairdresser { Name = "Nika"};

                _dbContext.Add(hairdresser1);
               _dbContext.Add(hairdresser2);
            }

            _dbContext.SaveChanges();
        }
    }
}
