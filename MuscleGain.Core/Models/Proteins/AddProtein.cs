using System.ComponentModel.DataAnnotations;
using static MuscleGain.Infrastructure.Data.DataConstants;

namespace MuscleGain.Core.Models.Proteins
{
    public class AddProtein
    {
        [Required]
        [StringLength(ProteinNameMaxLength,MinimumLength = ProteinNameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ProteinFlavorMaxLength, MinimumLength = ProteinFlavorMinLength)]
        public string Flavour { get; init; }

        [Required]
        [StringLength(ProteinGramsMaxLength, MinimumLength = ProteinGramsMinLength)]
        public string Grams { get; init; }

        [Required]
        [Range(ProteinPriceMinLength, ProteinPriceMaxLength)]
        public decimal? Price { get; init; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = ProteinDescriptionMinLength)]
        public string Description { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<ProteinCategoryViewModel>? Categories { get; set; }
    }
}
