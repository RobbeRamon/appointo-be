using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public class Hairdresser
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public OpeningHours OpeningHours { get; set; }
        public IList<Treatment> Treatments { get; set; }
        public IList<Appointment> Appointments { get; set; }

        #endregion


        protected Hairdresser() { }

        public Hairdresser(string name, IList<Treatment> treatments, IList<WorkDay> workDays)
        {
            Name = name;
            Treatments = treatments;
            OpeningHours = new OpeningHours { WorkDays = workDays };
            Appointments = new List<Appointment>();

            OpeningHours.WorkDays = workDays;

            //List<Time> hoursMonday = new List<Time>();
            //hoursMonday.Add(new Time(7, 30, 0));
            //hoursMonday.Add(new Time(12, 30, 0));
            //hoursMonday.Add(new Time(13, 30, 0));
            //hoursMonday.Add(new Time(18, 30, 0));

            //OpeningHours.EditHoursOfDay(DayOfWeek.Monday, hoursMonday);
        }

        public Appointment GetAppointment(int id)
        {
            return Appointments.FirstOrDefault(app => app.Id == id);
        }

        public Treatment GetTreatment(int id)
        {
            return Treatments.FirstOrDefault(tr => tr.Id == id);
        }

        public void AddTreatment(Treatment treatment)
        {
            Treatments.Add(treatment);
        }

        public bool AddAppointment(Appointment appointment)
        {

            if (NotInOpeningHours(appointment))
                return false;

            if (OverlappingWithAppointment(appointment))
                return false;

            Appointments.Add(appointment);

            return true;
            
        }

        public void RemoveAppointment(Appointment appointment)
        {
            Appointments.Remove(appointment);
        }

        private bool NotInOpeningHours(Appointment appointment)
        {
            IList<DateTime> openingHours = new List<DateTime>();
            IList<Time> workDay = OpeningHours.WorkDays.Single(wd => wd.Day == appointment.StartMoment.DayOfWeek).Hours;
            IList<bool> flags = new List<bool>();

            if (workDay.Count < 1)
                return true;

            for (int i = 0; i < workDay.Count; i++)
            {
                openingHours.Add(new DateTime(appointment.StartMoment.Year, appointment.StartMoment.Month, appointment.StartMoment.Day, workDay[i].Hour, workDay[i].Minute, workDay[i].Second));
            }

            for (int i = 0; i < openingHours.Count; i++)
            {
                if (i % 2 != 0)
                    continue;

                if (appointment.StartMoment <= openingHours[i] && appointment.EndMoment <= openingHours[i + 1])
                    flags.Add(true);
                else
                    flags.Add(false);
            }

            return (!flags.Contains(true));
        }

        private bool OverlappingWithAppointment(Appointment appointment)
        {
            foreach (Appointment a in Appointments)
            {
                if (!(appointment.StartMoment <= a.EndMoment && appointment.EndMoment <= a.StartMoment))
                    if (!(appointment.StartMoment >= a.EndMoment && appointment.EndMoment >= a.StartMoment))
                        return true;
            }

            return false;
        }

    }
}
