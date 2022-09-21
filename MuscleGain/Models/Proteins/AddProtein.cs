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
        [MinLength(ProteinQtyMinLength),MaxLength(ProteinQtyMaxLength)]
        public int? Quantity { get; init; }

        [Required]
        [MinLength(ProteinPriceMinLength),MaxLength(ProteinPriceMaxLength)]
        public decimal? Price { get; init; }

        [Required]
        public string Description { get; init; }

        [Required]
        public string ImageUrl { get; init; }

        public int CategoryId { get; set; }
    }
}
