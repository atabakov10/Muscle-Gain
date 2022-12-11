using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Cart;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Cart
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly MuscleGainDbContext dbContext;

		public ShoppingCartService(MuscleGainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task AddProteinInShoppingCart(int proteinId, string userId)
        {
            var protein = await this.dbContext.FindAsync<Protein>(proteinId);
			var user = await this.dbContext
				.Users
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

				user.ShoppingCart = shoppingCart;
				await this.dbContext.AddAsync(shoppingCart);
			}

			if (user.ShoppingCart.ShoppingCartProteins.Any(c => c.Id == proteinId))
			{
				return;
			}

			user.ShoppingCart.ShoppingCartProteins.Add(protein);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteAllProteinsFromShoppingCart(string userId)
		{
			var user = await this.dbContext
				 .Users
				 .Where(u => u.Id == userId)
				 .Include(sc => sc.ShoppingCart)
				 .ThenInclude(c => c.ShoppingCartProteins)
				 .FirstOrDefaultAsync();

			if (user == null)
			{
				return;
			}

			user.ShoppingCart.ShoppingCartProteins.Clear();
			await this.dbContext.SaveChangesAsync();
		}

		public async Task DeleteProteinFromShoppingCart(int proteinId, string userId)
		{
			var user = await this.dbContext
				.Users
				.Where(u => u.Id == userId)
				.Include(sc => sc.ShoppingCart)
				.ThenInclude(c => c.ShoppingCartProteins)
				.FirstOrDefaultAsync();

			var protein = user.ShoppingCart.ShoppingCartProteins.FirstOrDefault(c => c.Id == proteinId);

			if (user == null || protein == null)
			{
				return;
			}

			user.ShoppingCart.ShoppingCartProteins.Remove(protein);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<ShoppingCartViewModel> GetShoppingCart(string userId)
		{
			var user = await this.dbContext
				.Users
				.Where(u => u.Id == userId)
				.Include(sc => sc.ShoppingCart)
				.ThenInclude(c => c.ShoppingCartProteins.Where(x=> x.IsDeleted == false && x.IsApproved == true && x.OrderId == null))
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
