using Appointo_BE.Models;
using System.ComponentModel.DataAnnotations;

namespace Appointo_BE.DTOs
{
    public class TreatmentDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public TimeSpanDTO Duration { get; set; }
        [Required]
        public TreatmentCategory Category { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
