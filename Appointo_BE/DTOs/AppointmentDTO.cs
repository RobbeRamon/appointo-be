using Appointo_BE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class AppointmentDTO
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public IList<int> TreatmentIds { get; set; }
    }
}
