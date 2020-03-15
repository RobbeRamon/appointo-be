using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Model
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

        public Hairdresser()
        {
            OpeningHours = new OpeningHours();
            List<Time> hoursMonday = new List<Time>();
            hoursMonday.Add(new Time(7, 30, 0));
            hoursMonday.Add(new Time(12, 30, 0));
            hoursMonday.Add(new Time(13, 30, 0));
            hoursMonday.Add(new Time(18, 30, 0));

            OpeningHours.EditHoursOfDay(Day.MONDAY, hoursMonday);
        }
    }
}
