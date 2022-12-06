using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Reviews
{
    public class EditReviewViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Comment { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime LastUpdate { get; set; }

        public string UserId { get; set; }
    }
}
