using Appointo_BE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Appointo_BE.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public DateTime StartMoment { get; set; }
        [Required]
        public IList<Treatment> Treatments { get; set; }
    }
}
