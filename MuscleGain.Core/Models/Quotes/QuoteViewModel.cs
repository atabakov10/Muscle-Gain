using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Quotes
{
	public class QuoteViewModel: AddQuoteViewModel
	{
		[Display(Name = "Full name")]
		public string PublisherFullName { get; set; } = null!;

	}
}
