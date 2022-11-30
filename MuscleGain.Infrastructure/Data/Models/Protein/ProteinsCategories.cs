namespace MuscleGain.Infrastructure.Data.Models.Protein
{
    public class ProteinsCategories
    {
        public int Id { get; init; }

        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }
        public virtual ProteinsCategories? Parent { get; set; }

		public ICollection<Protein> Proteins { get; init; } = new HashSet<Protein>();

        public bool IsDeleted { get; set; }


    }
}
