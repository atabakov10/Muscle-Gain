using Microsoft.EntityFrameworkCore;
using MuscleGain.Contracts;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Models.ShoppingCart;

namespace MuscleGain.Services.ShoppingCart
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly IRepository repo;

		public ShoppingCartService(IRepository repo)
		{
			this.repo = repo;
		}

		public async Task AddProteinInShoppingCart(int proteinId, string userId)
		{
			var protein = await this.repo.GetByIdAsync<Protein>(proteinId);
			var user = await this.repo
				.All<ApplicationUser>()
				.Where(u => u.Id == userId)
				.Include(sc => sc.ShoppingCart)
				.ThenInclude(c => c.ShoppingCartProteins)
				.FirstOrDefaultAsync();

			if (protein == null || user == null)
			{
				return;
			}

			if (user.ShoppingCartId == null)
			{
				var shoppingCart = new Infrastructure.Data.Models.Cart.ShoppingCart()
				{
					UserId = user.Id
				};

				await this.repo.AddAsync(shoppingCart);
				user.ShoppingCartId = shoppingCart.Id;
			}

			if (user.ShoppingCart.ShoppingCartProteins.Any(c => c.Id == proteinId))
			{
				return;
			}

			user.ShoppingCart.ShoppingCartProteins.Add(protein);
			await this.repo.SaveChangesAsync();
		}

		public async Task DeleteAllProteinsFromShoppingCart(string userId)
		{
			var user = await this.repo
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
			await this.repo.SaveChangesAsync();
		}

		public async Task DeleteProteinFromShoppingCart(int proteinId, string userId)
		{
			var user = await this.repo
				.All<ApplicationUser>()
				.Where(u => u.Id == userId)
				.Include(sc => sc.ShoppingCart)
				.ThenInclude(c => c.ShoppingCartProteins)
				.FirstOrDefaultAsync();

			var course = user.ShoppingCart.ShoppingCartProteins.FirstOrDefault(c => c.Id == proteinId);

			if (user == null || course == null)
			{
				return;
			}

			user.ShoppingCart.ShoppingCartProteins.Remove(course);
			await this.repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<ShoppingCartProteinViewModel>> GetAllShoppingCartProteins(string userId)
		{
			var user = await this.repo
				.All<ApplicationUser>()
				.Where(u => u.Id == userId)
				.Include(sc => sc.ShoppingCart)
				.ThenInclude(c => c.ShoppingCartProteins)
				.FirstOrDefaultAsync();

			if (user == null || user.ShoppingCartId == null)
			{
				return null;
			}

			var courses = user.ShoppingCart?.ShoppingCartProteins.Select(c => new ShoppingCartProteinViewModel()
			{
				Id = c.Id,
				Name = c.Name,
				ImageUrl = c.ImageUrl,
				Price = (decimal)c.Price
			}).ToList();

			return courses;
		}
	}
}
