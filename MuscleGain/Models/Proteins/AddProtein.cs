using System.ComponentModel.DataAnnotations;
using static MuscleGain.Infrastructure.Data.DataConstants;

namespace MuscleGain.Models.Proteins
{
    public class AddProtein
    {
        public string Name { get; set; }

        [Required]
        [Range(ProteinGramsMinLength, ProteinGramsMaxLength)]
        public int? Grams { get; init; }

        [Required]
        [StringLength(ProteinFlavorMaxLength, MinimumLength = ProteinFlavorMinLength)]
        public string Flavour { get; init; }

        [Required]
        [Range(ProteinPriceMinLength, ProteinPriceMaxLength)]
        public decimal? Price { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; init; }

        public int CategoryId { get; set; }
    }
}
