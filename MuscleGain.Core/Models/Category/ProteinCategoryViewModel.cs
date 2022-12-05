using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Category
{
    public class ProteinCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(ModelConstants.CategoryNameMaxLength)]
        public string Name { get; set; } = null!;

    }
}
