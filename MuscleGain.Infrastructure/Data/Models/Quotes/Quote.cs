using MuscleGain.Infrastructure.Data.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Infrastructure.Data.Models.Quotes
{
    public class Quote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(DataConstants.QuoteTextMaxLength)]
        public string Text { get; set; } = null!;

        [StringLength(DataConstants.QuoteAuthorNameMaxLength)]
        public string? AuthorName { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

		public bool IsDeleted { get; set; }
    }
}
