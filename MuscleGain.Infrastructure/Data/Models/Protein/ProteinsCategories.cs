namespace MuscleGain.Infrastructure.Data.Models.Protein
{
    public class ProteinsCategories
    {
        public int Id { get; init; }

        public string Name { get; set; } = null!;

        public ICollection<Protein> Proteins { get; init; } = new HashSet<Protein>();

        public bool IsDeleted { get; set; }


    }
}
