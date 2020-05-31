using Appointo_BE.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Appointo_BE.DTOs
{
    public class WorkDaysDTO2
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
