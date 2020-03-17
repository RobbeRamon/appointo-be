using Appointo_BE.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class HairdresserDTO
    {
        public string Name { get; set; }
        public IList<TreatmentDTO> Treatments { get; set; }
    }
}
