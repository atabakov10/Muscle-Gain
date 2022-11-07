using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static MuscleGain.Infrastructure.Data.DataConstants;

namespace MuscleGain.Infrastructure.Data.Models.Protein
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
        [MaxLength(ProteinPriceMaxLength)]
        public decimal? Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        //[Comment("Product is active")]
        //public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }

        public ProteinsCategories ProteinCategory { get; init; }


    }
}
