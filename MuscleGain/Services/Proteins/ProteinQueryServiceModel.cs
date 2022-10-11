using MuscleGain.Models.Proteins;

namespace MuscleGain.Services.Proteins
{
    public class ProteinQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ProteinsPerPage { get; init; }

        public int TotalProteins { get; set; }

        public IEnumerable<ProteinListingViewModel> Proteins { get; set; }
    }
}
