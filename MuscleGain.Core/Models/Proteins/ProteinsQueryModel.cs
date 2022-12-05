using System.ComponentModel.DataAnnotations;
using MuscleGain.Core.Services.Proteins;

namespace MuscleGain.Core.Models.Proteins
{
	public class ProteinsQueryModel
    {
        public const int ProteinsPerPage = ModelConstants.MaxProteinsPerPage;
        public string Category { get; set; } = null!;

        [Display(Name = ModelConstants.SearchDisplayName)]
        public string? SearchTerm { get; init; }

        public ProteinSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = ModelConstants.CurrentPage;

        public int TotalProteins { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ProteinServiceModel> Proteins { get; set; }
    }
}
