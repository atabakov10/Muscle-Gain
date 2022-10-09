using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MuscleGain.Infrastructure.Data.DataConstants;

namespace MuscleGain.Infrastructure.Data.Models
{
    public class Protein
    {
        [Key]
        public int Id { get; init; }
        
        [Required]
        [StringLength(ProteinNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ProteinGramsMaxLength)]
        public string Grams { get; set; }

        [Required]
        [StringLength(ProteinFlavorMaxLength)]
        public string Flavour { get; set; }

        [Required]
        //[StringLength(ProteinPriceMaxLength)]
        [MaxLength(ProteinPriceMaxLength)]
        public decimal? Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public ProteinsCategories ProteinCategory { get; init; }
        
    }
}
