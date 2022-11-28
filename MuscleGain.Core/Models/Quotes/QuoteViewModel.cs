using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Quotes
{
	public class QuoteViewModel
	{ 
		public int Id { get; set; }

		[Required]
		[StringLength(2048, MinimumLength = 5)]
		public string Text { get; set; } = null!;

		[StringLength(30)]
		[Display(Name = "Author")]
		public string? AuthorName { get; set; }
	}
}
