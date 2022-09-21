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
        [Range(ProteinGramsMinLength, ProteinGramsMaxLength)]
        public int? Grams { get; set; }

        [Required]
        [StringLength(ProteinFlavorMaxLength, MinimumLength = ProteinFlavorMinLength)]
        public string Flavour { get; set; }

        [Required]
        [MinLength(ProteinPriceMinLength),MaxLength(ProteinPriceMaxLength)]
        public decimal? Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public ProteinsCategories ProteinCategory { get; init; }
        
    }
}
