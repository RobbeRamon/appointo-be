using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public interface IHairdresserRepository
    {
        Hairdresser GetBy(int id);
        Hairdresser GetByEmail(string email);
        IEnumerable<Hairdresser> GetAll();
        IEnumerable<Hairdresser> GetBy(string name = null);
        void Add(Hairdresser hairdresser);
        void Delete(Hairdresser hairdresser);
        void Update(Hairdresser hairdresser);
        void SaveChanges();
    }
}
