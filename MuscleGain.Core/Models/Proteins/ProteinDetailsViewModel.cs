using MuscleGain.Core.Models.Reviews;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Models.Proteins
{
    public class ProteinDetailsViewModel
    {
        public int Id { get; init; }

        public string Name { get; set; }

        public string Flavour { get; init; }

       
        public string Grams { get; init; }

    
        public decimal? Price { get; init; }

     
        public string Description { get; init; }
        
        public string ImageUrl { get; init; }

        public ICollection<ReviewViewModel> Reviews { get; set; } = new HashSet<ReviewViewModel>();

        public string Category { get; set; }
	}
}
