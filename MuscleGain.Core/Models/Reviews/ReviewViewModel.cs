namespace MuscleGain.Core.Models.Reviews
{
	public class ReviewViewModel
	{
		public string Comment { get; set; } = null!;

		public double Rating { get; set; }

		public string UserFullName { get; set; } = null!;

		public string DateOfPublication { get; set; } = null!;

		public string? ImageUrl { get; set; }
	}
}
