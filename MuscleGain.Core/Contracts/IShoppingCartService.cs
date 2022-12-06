using MuscleGain.Core.Models.Cart;

namespace MuscleGain.Core.Contracts
{
    public interface IShoppingCartService
    {
        Task AddProteinInShoppingCart(int proteinId, string userId);

        Task DeleteAllProteinsFromShoppingCart(string userId);

        Task DeleteProteinFromShoppingCart(int proteinId, string userId);

        Task<ShoppingCartViewModel> GetShoppingCart(string userId);
    }
}
