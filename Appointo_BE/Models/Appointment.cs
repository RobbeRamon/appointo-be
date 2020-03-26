using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public IList<Treatment> Treatments { get; set; }
        public DateTime StartMoment { get; set; }
        public DateTime EndMoment => StartMoment.Add(TotalDuration);

        protected Appointment(){}

        public Appointment(IList<Treatment> treatments, DateTime startMoment)
        {
            Treatments = treatments;
            StartMoment = startMoment;
        }

        public TimeSpan TotalDuration
        {
            get 
            {
                TimeSpan totalDuration = new TimeSpan();
                foreach (Treatment tr in Treatments) totalDuration = totalDuration.Add(tr.Duration);
                return totalDuration;
            }
        }

    }   
}
