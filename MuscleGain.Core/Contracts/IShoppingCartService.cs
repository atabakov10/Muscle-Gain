using MuscleGain.Core.Models.ShoppingCart;

namespace MuscleGain.Core.Contracts
{
	public interface IShoppingCartService
	{
		Task AddProteinInShoppingCart(int proteinId, string userId);

		Task<IEnumerable<ShoppingCartProteinViewModel>> GetAllShoppingCartProteins(string userId);

		Task DeleteProteinFromShoppingCart(int proteinId, string userId);

		Task DeleteAllProteinsFromShoppingCart(string userId);
	}
}
