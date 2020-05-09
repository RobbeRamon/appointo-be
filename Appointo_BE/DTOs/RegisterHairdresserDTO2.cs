using Appointo_BE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class RegisterHairdresserDTO2
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<TreatmentDTO> Treatments { get; set; }
        public WorkDaysDTO2 WorkDays { get; set; }
    }
}
