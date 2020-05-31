using System.Collections.Generic;

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
