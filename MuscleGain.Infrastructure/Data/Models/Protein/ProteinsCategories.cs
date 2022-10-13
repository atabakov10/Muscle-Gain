namespace MuscleGain.Infrastructure.Data.Models.Protein
{
    public class ProteinsCategories
    {
        public int Id { get; init; }
        public string Name { get; set; } = null!;
        public IEnumerable<Protein> Proteins { get; init; } = new List<Protein>();
    }
}
