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
        public IList<Time> Monday { get; set; }
        [Required]
        public IList<Time> Tuesday { get; set; }
        [Required]
        public IList<Time> Wednesday { get; set; }
        [Required]
        public IList<Time> Thursday { get; set; }
        [Required]
        public IList<Time> Friday { get; set; }
        [Required]
        public IList<Time> Saturday { get; set; }
        [Required]
        public IList<Time> Sunday { get; set; }
    }
}
