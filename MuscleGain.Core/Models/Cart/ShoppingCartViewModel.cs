namespace MuscleGain.Core.Models.Cart
{
	public class ShoppingCartViewModel
	{
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string UserFullName { get; set; } = null!;

        public decimal TotalPrice => Proteins.Sum(c => c.Price);

        public ICollection<ShoppingCartProteinViewModel> Proteins { get; set; } = new List<ShoppingCartProteinViewModel>();
    }
}
