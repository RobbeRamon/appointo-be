using System.ComponentModel.DataAnnotations;

namespace Appointo_BE.DTOs
{
    public class TimeSpanDTO
    {
        [Required]
        public int Hours { get; set; }
        [Required]
        public int Minutes { get; set; }
        [Required]
        public int Seconds { get; set; }
    }
}
