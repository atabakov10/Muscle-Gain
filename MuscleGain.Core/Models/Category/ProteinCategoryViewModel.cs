using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Category
{
    public class ProteinCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public virtual ICollection<ProteinCategoryViewModel> ProteinCategories { get; set; } = new HashSet<ProteinCategoryViewModel>();
    }
}
