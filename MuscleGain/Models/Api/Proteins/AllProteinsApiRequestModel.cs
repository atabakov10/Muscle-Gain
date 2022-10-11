using MuscleGain.Models.Proteins;

namespace MuscleGain.Models.Api.Proteins
{
    public class AllProteinsApiRequestModel
    {
        public string Flavour { get; set; }

        public string SearchTerm { get; set; }

        public ProteinSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int ProteinsPerPage { get; init; } = 10;
    }
}
