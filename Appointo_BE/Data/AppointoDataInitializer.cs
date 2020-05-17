using Appointo_BE.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Data
{
    public class AppointoDataInitializer
    {
        private readonly AppointoDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AppointoDataInitializer(AppointoDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeDataAsync()
        {
            _dbContext.Database.EnsureDeleted();
            
            if(_dbContext.Database.EnsureCreated())
            {
                // Hairlounge Marlies

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
                

                Appointment appointment = new Appointment(new List<Treatment>() { treatment }, new DateTime(2020, 5, 27, 11,30,0), "Robbe", "Ramon");

                Hairdresser hairdresser1 = new Hairdresser("Hairlounge Marlies", "hairloungemarlies@gmail.com", treatments, workDays);


                hairdresser1.AddAppointment(appointment);

                _dbContext.Add(hairdresser1);

                await CreateUser(hairdresser1.Email, "P@ssword1111");



                // Alexa

                List<TimeRange> workdayB = new List<TimeRange>();
                workdayB.Add(new TimeRange(new Time(7, 30, 0), new Time(12, 30, 0)));
                workdayB.Add(new TimeRange(new Time(13, 30, 0), new Time(18, 30, 0)));


                IList<WorkDay> workDaysB = new List<WorkDay>
                {
                    new WorkDay(DayOfWeek.Monday, workdayB),
                    new WorkDay(DayOfWeek.Tuesday, workdayB.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Wednesday, workdayB.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Thursday, workdayB.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Friday, workdayB.Select(wd => (TimeRange)wd.Clone()).ToList()),
                    new WorkDay(DayOfWeek.Saturday, new List<TimeRange>()),
                    new WorkDay(DayOfWeek.Sunday, new List<TimeRange>())
                };

                IList<Treatment> treatmentsB = new List<Treatment>();
                Treatment treatmentB = new Treatment("Knippen", new TimeSpan(0, 30, 0), TreatmentCategory.MEN, 25);
                Treatment treatment2B = new Treatment("Knippen", new TimeSpan(0, 30, 0), TreatmentCategory.WOMEN, 35);
                Treatment treatment3B = new Treatment("Knippen", new TimeSpan(0, 30, 0), TreatmentCategory.CHILDREN, 15);
                treatmentsB.Add(treatmentB);
                treatmentsB.Add(treatment2B);
                treatmentsB.Add(treatment3B);


                Appointment appointmentB = new Appointment(new List<Treatment>() { treatmentB }, new DateTime(2020, 5, 27, 11, 30, 0), "Robbe", "Ramon");

                Hairdresser hairdresser1B = new Hairdresser("Alexa", "alexa@gmail.com", treatmentsB, workDaysB);


                hairdresser1B.AddAppointment(appointmentB);

                _dbContext.Add(hairdresser1B);

                await CreateUser(hairdresser1B.Email, "P@ssword1111");

            }

            _dbContext.SaveChanges();
        }

        private async Task CreateUser(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            await _userManager.CreateAsync(user, password);
        }
    }
}
