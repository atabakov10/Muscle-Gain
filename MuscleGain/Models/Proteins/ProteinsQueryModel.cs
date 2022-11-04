using System.ComponentModel.DataAnnotations;
using MuscleGain.Services.Proteins;

namespace MuscleGain.Models.Proteins
{
	public class ProteinsQueryModel
    {
        public const int ProteinsPerPage = 3;
        public string Category { get; set; } = null!;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public ProteinSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int TotalProteins { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ProteinServiceModel> Proteins { get; set; }
    }
}
