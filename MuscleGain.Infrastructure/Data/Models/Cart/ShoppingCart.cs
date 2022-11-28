using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Infrastructure.Data.Models.Cart
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public ICollection<Protein.Protein> ShoppingCartProteins { get; set; } = new HashSet<Protein.Protein>();

    }
}
