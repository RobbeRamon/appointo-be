using Appointo_BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class HairdresserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IList<Treatment> Treatments { get; set; }
        public WorkDaysDTO2 WorkDays { get; set; }
    }
}
