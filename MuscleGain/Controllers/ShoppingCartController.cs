using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using System.Security.Claims;
using MuscleGain.Core.Contracts;

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

		public async Task<IActionResult> Add([FromRoute] int proteinId)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			await this.shoppingCartService.AddProteinInShoppingCart(proteinId, userId);

			//TempData[MessageConstant.SuccessMessage] = "Successfully added protein to Shopping cart";

			return RedirectToAction("All", "Protein");
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
