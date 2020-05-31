using System;
using System.Collections.Generic;

namespace Appointo_BE.Models
{
    public class WorkDay
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public IList<TimeRange> Hours { get; set; }

        protected WorkDay() { }

        public WorkDay(DayOfWeek day, IList<TimeRange> hours)
        {
            Day = day;
            Hours = hours;
        }
    }
}
