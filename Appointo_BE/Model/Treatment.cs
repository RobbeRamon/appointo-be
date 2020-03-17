using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.Model
{
    public class Treatment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }

        public Treatment(string name, TimeSpan duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}
