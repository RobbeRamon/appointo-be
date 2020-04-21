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

                List<TimeRange> workday = new List<TimeRange>();
                workday.Add(new TimeRange(new Time(7, 30, 0), new Time(12, 30, 0)));
                workday.Add(new TimeRange(new Time(13, 30, 0), new Time(18, 30, 0)));


                IList<WorkDay> workDays = new List<WorkDay>
                {
                    new WorkDay(DayOfWeek.Monday, workday),
                    new WorkDay(DayOfWeek.Tuesday, workday.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Wednesday, workday.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Thursday, workday.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Friday, workday.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Saturday, new List<TimeRange>()),
                    new WorkDay(DayOfWeek.Sunday, new List<TimeRange>())
                };

                IList<Treatment> treatments = new List<Treatment>();
                Treatment treatment = new Treatment("Knippen", new TimeSpan(0, 20, 0), TreatmentCategory.MEN, 20);
                Treatment treatment2 = new Treatment("Knippen", new TimeSpan(0, 20, 0), TreatmentCategory.WOMEN, 30);
                Treatment treatment3 = new Treatment("Knippen", new TimeSpan(0, 20, 0), TreatmentCategory.CHILDREN, 10);
                treatments.Add(treatment);
                treatments.Add(treatment2);
                treatments.Add(treatment3);
                

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
