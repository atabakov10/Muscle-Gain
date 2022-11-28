using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Cart;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Protein;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MuscleGain.Core.Services.Cart
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository repository;

        public ShoppingCartService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddProteinInShoppingCart(int proteinId, string userId)
        {
            var protein = await this.repository.GetByIdAsync<Protein>(proteinId);
            var user = await this.repository
                .All<ApplicationUser>()
                .Where(u => u.Id == userId)
                .Include(sc => sc.ShoppingCart)
                .ThenInclude(c => c.ShoppingCartProteins)
                .FirstOrDefaultAsync();

            if (protein == null || user == null)
            {
                throw new Exception("Protein Id or User Id is null");
            }


            if (user.ShoppingCartId == null)
            {
                var shoppingCart = new ShoppingCart()
                {
                    UserId = user.Id,
                };

                await this.repository.AddAsync(shoppingCart);
                user.ShoppingCartId = shoppingCart.Id;
            }

            if (user.ShoppingCart.ShoppingCartProteins.Any(c => c.Id == proteinId))
            {
                return;
            }

            user.ShoppingCart.ShoppingCartProteins.Add(protein);
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteAllProteinsFromShoppingCart(string userId)
        {
            var user = await this.repository
                 .All<ApplicationUser>()
                 .Where(u => u.Id == userId)
                 .Include(sc => sc.ShoppingCart)
                 .ThenInclude(c => c.ShoppingCartProteins)
                 .FirstOrDefaultAsync();

            if (user == null)
            {
                return;
            }

            user.ShoppingCart.ShoppingCartProteins.Clear();
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteProteinFromShoppingCart(int courseId, string userId)
        {
            var user = await this.repository
                .All<ApplicationUser>()
                .Where(u => u.Id == userId)
                .Include(sc => sc.ShoppingCart)
                .ThenInclude(c => c.ShoppingCartProteins)
                .FirstOrDefaultAsync();

            var course = user.ShoppingCart.ShoppingCartProteins.FirstOrDefault(c => c.Id == courseId);

            if (user == null || course == null)
            {
                return;
            }

            user.ShoppingCart.ShoppingCartProteins.Remove(course);
            await this.repository.SaveChangesAsync();
        }

        public async Task<ShoppingCartViewModel> GetShoppingCart(string userId)
        {
            var user = await this.repository
                .All<ApplicationUser>()
                .Where(u => u.Id == userId)
                .Include(sc => sc.ShoppingCart)
                .ThenInclude(c => c.ShoppingCartProteins)
                .FirstOrDefaultAsync();

            if (user == null || user.ShoppingCartId == null)
            {
                return null;
            }

            var proteins = user.ShoppingCart?.ShoppingCartProteins.Select(c => new ShoppingCartProteinViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                ImageUrl = c.ImageUrl,
                Price = c.Price,
            }).ToList();

            var cart = new ShoppingCartViewModel()
            {
                UserFullName = $"{user.FirstName} {user.LastName}",
                UserName = user.UserName,
                Email = user.Email,
                Proteins = proteins,
            };

            return cart;
        }
    }
}
