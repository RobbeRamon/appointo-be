using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public class OpeningHours
    {
        public int Id { get; set; }
        public IList<WorkDay> WorkDays { get; set; }

        public OpeningHours()
        {
            WorkDays = new List<WorkDay>();
            //FillHours();
        }

        //public OpeningHours(IList<WorkDay> workDays)
        //{

        //    FillHours();
        //}

        public void EditHoursOfDay(DayOfWeek day, List<TimeRange> hours)
        {
            this.WorkDays.Single(wd => wd.Day == day).Hours = hours;
        }

        //private void FillHours()
        //{
        //    var days = Enum.GetValues(typeof(DayOfWeek));

        //    foreach (DayOfWeek day in days)
        //        WorkDays.Add(new WorkDay(day));
        //}
    }
}
