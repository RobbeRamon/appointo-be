using Appointo_BE.Models;
using System.Collections.Generic;

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
