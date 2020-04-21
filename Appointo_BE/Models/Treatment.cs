using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Appointo_BE.Models
{
    public enum TreatmentCategory
    {
        MEN,
        WOMEN,
        CHILDREN
    }

    public class Treatment
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public TreatmentCategory Category { get; set; }
        public decimal Price { get; set; }

        protected Treatment() { }

        public Treatment(string name, TimeSpan duration, TreatmentCategory category, decimal price)
        {
            Name = name;
            Duration = duration;
            Category = category;
            Price = price;
        }
    }
}
