using Appointo_BE.Models;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class WorkDayDTO
    {

        protected WorkDayDTO(){}

        public WorkDayDTO(int dayId, List<TimeRange> hours)
        {
            DayId = dayId;
            Hours = hours;
        }


        [Required]
        public int DayId { get; set; }
        [Required]
        public List<TimeRange> Hours { get; set; }
    }
}
