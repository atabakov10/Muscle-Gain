using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Category
{
    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; } = null!;
    }
}
