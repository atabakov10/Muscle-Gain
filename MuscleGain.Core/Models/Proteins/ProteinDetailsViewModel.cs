using MuscleGain.Core.Models.Reviews;

namespace MuscleGain.Core.Models.Proteins
{
    public class ProteinDetailsViewModel
    {
        public int Id { get; init; }

        public string Name { get; set; } = null!;

        public string Flavour { get; init; } = null!;

        public string CreatorFullName { get; set; } = null!;
        public string Grams { get; init; }

        public string Email { get; set; } = null!;
        public decimal? Price { get; init; }

        public double? AvgRating { get; set; }

		public string Description { get; init; } = null!;
        
        public string ImageUrl { get; init; } = null!;

        public ICollection<ReviewViewModel> Reviews { get; set; } = new HashSet<ReviewViewModel>();

        public string Category { get; set; } = null!;
	}
}
