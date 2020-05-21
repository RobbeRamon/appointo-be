using Appointo_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Appointo_BE.Data.Repositories
{
    public class HairdresserRepository : IHairdresserRepository
    {
        private readonly DbSet<Hairdresser> _hairdressers;
        private readonly AppointoDbContext _context;

        public HairdresserRepository(AppointoDbContext context)
        {
            _context = context;
            _hairdressers = context.Hairdressers;
        }

        public Hairdresser GetBy(int id)
        {
            return _hairdressers
                .Include(hd => hd.Treatments)
                .Include(hd => hd.Appointments)
                    .ThenInclude(hd => hd.Treatments)
                    .ThenInclude(at => at.Treatment)
                .Include(hd => hd.OpeningHours)
                    .ThenInclude(hd => hd.WorkDays)
                    .ThenInclude(hd => hd.Hours)
                    .ThenInclude(h => h.StartTime)
                .Include(hd => hd.OpeningHours)
                    .ThenInclude(hd => hd.WorkDays)
                    .ThenInclude(hd => hd.Hours)
                    .ThenInclude(h => h.EndTime)
                .SingleOrDefault(hd => hd.Id == id);
        }

        public Hairdresser GetByEmail(string email)
        {
            return _hairdressers.Where(hd => hd.Email == email)
                                .Include(hd => hd.Treatments)
                                .Include(hd => hd.Appointments)
                                    .ThenInclude(hd => hd.Treatments)
                                    .ThenInclude(at => at.Treatment)
                                .Include(hd => hd.OpeningHours)
                                    .ThenInclude(hd => hd.WorkDays)
                                    .ThenInclude(hd => hd.Hours)
                                    .ThenInclude(h => h.StartTime)
                                .Include(hd => hd.OpeningHours)
                                    .ThenInclude(hd => hd.WorkDays)
                                    .ThenInclude(hd => hd.Hours)
                                    .ThenInclude(h => h.EndTime)
                                .SingleOrDefault();

        }

        public IEnumerable<Hairdresser> GetBy(string name = null)
        {
            return _hairdressers.Where(hd => hd.Name.StartsWith(name))
                                .Include(hd => hd.Treatments)
                                .Include(hd => hd.Appointments)
                                    .ThenInclude(hd => hd.Treatments)
                                    .ThenInclude(at => at.Treatment)
                                .Include(hd => hd.OpeningHours)
                                    .ThenInclude(hd => hd.WorkDays)
                                    .ThenInclude(hd => hd.Hours)
                                    .ThenInclude(h => h.StartTime)
                                .Include(hd => hd.OpeningHours)
                                    .ThenInclude(hd => hd.WorkDays)
                                    .ThenInclude(hd => hd.Hours)
                                    .ThenInclude(h => h.EndTime)
                                .ToList();
        }
    
        public IEnumerable<Hairdresser> GetAll()
        {
            return _hairdressers
                .Include(hd => hd.Treatments)
                .AsNoTracking()
                .ToList();
        }

        public void Add(Hairdresser hairdresser)
        {
            _hairdressers.Add(hairdresser);
        }

        public void Delete(Hairdresser hairdresser)
        {
            _hairdressers.Remove(hairdresser);
        }

        public void Update(Hairdresser hairdresser)
        {
            _hairdressers.Update(hairdresser);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
