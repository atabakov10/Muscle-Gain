using MuscleGain.Models.Reviews;
using MuscleGain.Services.Proteins;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MuscleGain.Models.Proteins
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

        public int CategoryId { get; set; }
	}
}
