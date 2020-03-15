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
        public Dictionary<string, List<Time>> Hours { get; set; }

        public OpeningHours()
        {
            Hours = new Dictionary<String, List<Time>>();
            FillHours();
        }

        public void EditHoursOfDay(Day day, List<Time> hours)
        {
            this.Hours[day.ToString()] = hours;
        }

        private void FillHours()
        {
            Hours.Add(Day.MONDAY.ToString(), null);
            Hours.Add(Day.TUESDAY.ToString(), null);
            Hours.Add(Day.WEDNESDAY.ToString(), null);
            Hours.Add(Day.THURSDAY.ToString(), null);
            Hours.Add(Day.FRIDAY.ToString(), null);
            Hours.Add(Day.SATERDAY.ToString(), null);
            Hours.Add(Day.SUNDAY.ToString(), null);
        }
    }
}
