using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public class Treatment
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }

        protected Treatment() { }

        public Treatment(string name, TimeSpan duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}
