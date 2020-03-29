﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Appointo_BE.DTOs
{
    public class TreatmentDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public TimeSpanDTO Duration { get; set; }
    }
}