using System;
using System.Collections.Generic;

namespace Appointo_BE.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IList<AppointmentTreatment> Treatments { get; set; }
        public DateTime StartMoment { get; set; }
        public DateTime EndMoment => StartMoment.Add(TotalDuration);
        public TimeSpan TotalDuration
        {
            get
            {
                TimeSpan totalDuration = new TimeSpan();
                foreach (AppointmentTreatment tr in Treatments) totalDuration = totalDuration.Add(tr.Treatment.Duration);
                return totalDuration;
            }
        }

        protected Appointment(){}

        public Appointment(IList<Treatment> treatments, DateTime startMoment, string firstname, string lastname)
        {
            Treatments = new List<AppointmentTreatment>();
            foreach (Treatment tr in treatments) Treatments.Add(new AppointmentTreatment(this, tr));

            StartMoment = startMoment;
            Firstname = firstname;
            Lastname = lastname;
        }



    }   
}
