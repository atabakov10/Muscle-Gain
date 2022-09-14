using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Infrastructure.Data.Models
{
    public class Protein
    {
        public Protein()
        {
            
        }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string? Name { get; set; }
        [Required]
        [MinLength(0)]
        [MaxLength(10000)]
        public decimal Price { get; set; }
    }
}
