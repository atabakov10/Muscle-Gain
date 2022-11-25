using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
