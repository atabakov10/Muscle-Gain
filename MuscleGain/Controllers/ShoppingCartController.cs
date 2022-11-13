using Microsoft.AspNetCore.Mvc;
using MuscleGain.Contracts;
using MuscleGain.Core.Constants;
using System.Security.Claims;

namespace MuscleGain.Controllers
{
	public class ShoppingCartController : BaseController
	{
		private readonly IShoppingCartService shoppingCartService;

		public ShoppingCartController(IShoppingCartService shoppingCartService)
		{
			this.shoppingCartService = shoppingCartService;
		}

		public async Task<IActionResult> Index()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			var proteins = await this.shoppingCartService.GetAllShoppingCartProteins(userId);

			return View(proteins);
		}

		public async Task<IActionResult> Add([FromRoute] int id)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			await this.shoppingCartService.AddProteinInShoppingCart(id, userId);

			TempData[MessageConstant.SuccessMessage] = "Successfully added protein to Shopping cart";

			return RedirectToAction("All", "Proteins");
		}

		public async Task<IActionResult> Delete(int id)
		{

			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			await this.shoppingCartService.DeleteProteinFromShoppingCart(id, userId);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> DeleteAll()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			await this.shoppingCartService.DeleteAllProteinsFromShoppingCart(userId);

			return RedirectToAction(nameof(Index));
		}
	}
}
