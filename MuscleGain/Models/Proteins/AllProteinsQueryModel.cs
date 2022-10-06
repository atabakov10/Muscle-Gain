using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Models.Proteins
{
	public class AllProteinsQueryModel
    {
        public string Name { get; init; }
        public IEnumerable<string> Names { get; init; }

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public ProteinSorting Sorting { get; init; }
        public IEnumerable<ProteinListingViewModel> Proteins { get; init; }
    }
}
