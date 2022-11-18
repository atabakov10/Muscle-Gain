namespace MuscleGain.Core.Services.Proteins
{
    public class ProteinQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ProteinsPerPage { get; init; } = 3;

        public int TotalProteins { get; set; }

        public IEnumerable<ProteinServiceModel> Proteins { get; set; }
    }
}
