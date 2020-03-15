using Appointo_BE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Data.Repositories
{
    public class HairdresserRepository : IHairdresserRepository
    {
        public Hairdresser GetBy(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hairdresser> GetBy(string name = null, string location = null)
        {
            throw new NotImplementedException();
        }
    
        public IEnumerable<Hairdresser> GetAll()
        {
            Hairdresser hairdresser1 = new Hairdresser { Name = "Hairlounge Marlies", Id = 0};
            Hairdresser hairdresser2 = new Hairdresser { Name = "Nika", Id = 1 };

            List<Hairdresser> hairdressers = new List<Hairdresser>();
            hairdressers.Add(hairdresser1);
            hairdressers.Add(hairdresser2);

            return hairdressers;
        }

        public void Add(Hairdresser hairdresser)
        {
            throw new NotImplementedException();
        }

        public void Delete(Hairdresser hairdresser)
        {
            throw new NotImplementedException();
        }

        public void Update(Hairdresser hairdresser)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }


    }
}
