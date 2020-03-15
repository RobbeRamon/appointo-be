using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Model
{
    public enum Day
    {
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATERDAY,
        SUNDAY
    }

    public class OpeningHours
    {
        public int Id { get; set; }
        public IList<WorkDay> WorkDays { get; set; }

        public OpeningHours()
        {
            WorkDays = new List<WorkDay>();
            FillHours();
        }

        public void EditHoursOfDay(Day day, List<Time> hours)
        {
            this.WorkDays.Single(wd => wd.Day == day).Hours = hours;
        }

        private void FillHours()
        {
            var days = Enum.GetValues(typeof(Day));

            foreach (Day day in days)
                WorkDays.Add(new WorkDay(day));
        }
    }
}
