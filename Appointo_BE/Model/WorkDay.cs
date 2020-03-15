using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Model
{
    public class WorkDay
    {
        public int Id { get; set; }
        public Day Day { get; set; }
        public IList<Time> Hours { get; set; }

        public WorkDay(Day day)
        {
            Day = day;
        }
    }
}
