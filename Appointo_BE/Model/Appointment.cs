using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public IList<Treatment> Treatments { get; set; }

        public TimeSpan TotalDuration
        {
            get 
            {
                TimeSpan totalDuration = new TimeSpan();
                foreach (Treatment tr in Treatments) totalDuration.Add(tr.Duration);
                return totalDuration;
            }
        }

    }   
}
