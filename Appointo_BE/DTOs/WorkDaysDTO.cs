using Appointo_BE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class WorkDaysDTO
    {
        [Required]
        public IList<TimeRange> Monday { get; set; }
        [Required]
        public IList<TimeRange> Tuesday { get; set; }
        [Required]
        public IList<TimeRange> Wednesday { get; set; }
        [Required]
        public IList<TimeRange> Thursday { get; set; }
        [Required]
        public IList<TimeRange> Friday { get; set; }
        [Required]
        public IList<TimeRange> Saturday { get; set; }
        [Required]
        public IList<TimeRange> Sunday { get; set; }
    }
}
