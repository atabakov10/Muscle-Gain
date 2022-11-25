using MuscleGain.Core.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Cart;

namespace MuscleGain.Core.Contracts
{
    public interface IShoppingCartService
    {
        Task AddProteinInShoppingCart(int proteinId, string userId);

        Task DeleteAllProteinsFromShoppingCart(string userId);

        Task DeleteProteinFromShoppingCart(int courseId, string userId);

        Task<ShoppingCartViewModel> GetShoppingCart(string userId);
    }
}
