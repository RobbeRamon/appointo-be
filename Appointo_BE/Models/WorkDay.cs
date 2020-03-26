using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public class WorkDay
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public IList<Time> Hours { get; set; }

        protected WorkDay() { }

        public WorkDay(DayOfWeek day, IList<Time> hours)
        {
            Day = day;
            Hours = hours;
        }
    }
}
