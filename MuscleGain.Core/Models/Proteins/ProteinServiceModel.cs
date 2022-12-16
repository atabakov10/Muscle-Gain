namespace MuscleGain.Core.Models.Proteins
{
    public class ProteinServiceModel
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public string Grams { get; init; } = null!;
        public string Flavour { get; init; }= null!;
        public decimal? Price { get; init; }
        public string ImageUrl { get; init; }= null!;
        public string Category { get; init; }= null!;
    }
}
