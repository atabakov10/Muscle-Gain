using System.ComponentModel.DataAnnotations;
using MuscleGain.Core.Models.Category;
using Newtonsoft.Json.Serialization;
using static MuscleGain.Infrastructure.Data.DataConstants;

namespace MuscleGain.Core.Models.Proteins
{
    public class AddProtein
    {
        [Required(ErrorMessage = "The name is required!")]
        [StringLength(ProteinNameMaxLength, MinimumLength = ProteinNameMinLength, ErrorMessage = "The name should be at least 4 letters long and maximum of 30 letters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The flavour is required!")]
        [StringLength(ProteinFlavorMaxLength, MinimumLength = ProteinFlavorMinLength, ErrorMessage = "The flavour should be at least 3 letters long and maximum of 30 letters.")]
        public string Flavour { get; init; } = null!;

        [Required(ErrorMessage = "The grams are required!")]
        [StringLength(ProteinGramsMaxLength, MinimumLength = ProteinGramsMinLength, ErrorMessage = "Grams should be a number with kg/g at the end of it.")]
        public string Grams { get; init; } = null!;

        [Required(ErrorMessage = "The price is required!")]
		[Range(ProteinPriceMinLength, ProteinPriceMaxLength, ErrorMessage = "The price should be a number.")]
        public decimal? Price { get; init; }

        [Required(ErrorMessage = "Description is required!")]
		[StringLength(int.MaxValue, MinimumLength = ProteinDescriptionMinLength, ErrorMessage = "Description should be a text with minimum of 10 letters.")]
        public string Description { get; init; } = null!;

        [Required]
        public string UserId { get; set; } = null!;


        [Required(ErrorMessage = "The image URL is required!")]
		[Display(Name = "Image URL")]
        [Url(ErrorMessage = "The Image should be a real URL.")]
        public string ImageUrl { get; init; } = null!;

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<ProteinCategoryViewModel>? Categories { get; set; }
    }
}
