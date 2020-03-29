using Appointo_BE.Models;
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

                List<Time> workday = new List<Time>();
                workday.Add(new Time(7, 30, 0));
                workday.Add(new Time(12, 30, 0));
                workday.Add(new Time(13, 30, 0));
                workday.Add(new Time(18, 30, 0));


                IList<WorkDay> workDays = new List<WorkDay>
                {
                    new WorkDay(DayOfWeek.Monday, workday),
                    new WorkDay(DayOfWeek.Tuesday, workday.Select(wd => (Time)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Wednesday, workday.Select(wd => (Time)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Thursday, workday.Select(wd => (Time)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Friday, workday.Select(wd => (Time)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Saturday, new List<Time>()),
                    new WorkDay(DayOfWeek.Sunday, new List<Time>())
                };

                IList<Treatment> treatments = new List<Treatment>();
                Treatment treatment = new Treatment("Knippen", new TimeSpan(0, 20, 0));
                treatments.Add(treatment);

                Appointment appointment = new Appointment(new List<Treatment>() { treatment }, new DateTime(2020, 3, 27, 11,30,0));

                Hairdresser hairdresser1 = new Hairdresser("Hairlounge Marlies", treatments, workDays);

                hairdresser1.AddTreatment(treatment);
                hairdresser1.AddAppointment(appointment);

                _dbContext.Add(hairdresser1);
            }

            _dbContext.SaveChanges();
        }
    }
}
