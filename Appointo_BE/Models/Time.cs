using System;
using System.ComponentModel.DataAnnotations;

namespace Appointo_BE.Models
{
    public class Time : ICloneable
    {
        public int Id { get; set; }
        [Required]
        public int Hour { get; set; }
        [Required]
        public int Minute { get; set; }
        [Required]
        public int Second { get; set; }

        protected Time() { }

        public Time(int hour, int minute, int second)
        {
            this.Hour = hour;
            this.Minute = minute;
            this.Second = second;
        }

        public object Clone()
        {
            return new Time(Hour, Minute, Second);
        }
    }
}
