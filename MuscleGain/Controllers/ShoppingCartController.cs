using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using System.Security.Claims;
using MuscleGain.Core.Models.Order;
using Stripe;

namespace MuscleGain.Controllers
{
	public class ShoppingCartController : BaseController
	{

		private readonly IShoppingCartService shoppingCartService;
		private readonly IOrderService orderService;
		private readonly IProteinService proteinService;
		private readonly ILogger logger;

		public ShoppingCartController(
			IShoppingCartService shoppingCartService,
			IOrderService orderService,
			IProteinService proteinService,
			ILogger<ShoppingCartController> logger)
		{
			this.shoppingCartService = shoppingCartService;
			this.orderService = orderService;
			this.proteinService = proteinService;
			this.logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			var shoppingCart = await this.shoppingCartService.GetShoppingCart(userId);

			return this.View(shoppingCart);
		}

		public async Task<IActionResult> Add([FromRoute]int id)
		{
			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				logger.LogError("Invalid user id");
				this.TempData[MessageConstant.ErrorMessage] = "Something went wrong..";
				return this.RedirectToAction("All", "Protein");
			}


			await this.shoppingCartService.AddProteinInShoppingCart(id, userId);


			this.TempData[MessageConstant.SuccessMessage] = "Successfully added protein to Shopping cart";

			return this.RedirectToAction("Index", "ShoppingCart");
		}

		public async Task<IActionResult> Delete(int id)
		{

			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				logger.LogError("Invalid protein Id");
				TempData[MessageConstant.ErrorMessage] = "Something went wrong..";
				throw new Exception();
			}
			await this.shoppingCartService.DeleteProteinFromShoppingCart(id, userId);

			return this.RedirectToAction(nameof(this.Index));
		}

		public async Task<IActionResult> DeleteAll()
		{
			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
			{
				logger.LogError("Invalid user id");
				this.TempData[MessageConstant.ErrorMessage] = "Something went wrong..";
				return this.RedirectToAction("All", "Protein");
			}
			await this.shoppingCartService.DeleteAllProteinsFromShoppingCart(userId);

			return this.RedirectToAction(nameof(this.Index));
		}

		public async Task<IActionResult> Summary()
		{
			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			var shoppingCart = await this.shoppingCartService.GetShoppingCart(userId);

			return this.View(shoppingCart);
		}

		[HttpPost]
		public async Task<IActionResult> Summary(string stripeToken)
		{
			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			var shoppingCart = await this.shoppingCartService.GetShoppingCart(userId);
			var order = new OrderViewModel()
			{
				Proteins = shoppingCart.Proteins,
				TotalPrice = shoppingCart.TotalPrice,
				OrderDate = DateTime.Now,
				OrderStatus = "pending",
				PaymentStatus = "pending",
			};

			order.Id = await this.orderService.AddOrder(order, userId);
			var options = new ChargeCreateOptions
			{
				Amount = Convert.ToInt32(order.TotalPrice * 100),
				Currency = "usd",
				Description = "Order ID : " + order.Id,
				Source = stripeToken,
			};

			var service = new ChargeService();
			Charge charge = service.Create(options);
			if (charge.Id == null)
			{
				order.PaymentStatus = "rejected";
			}
			else
			{
				order.TransactionId = charge.Id;
			}

			if (charge.Status.ToLower() == "succeeded")
			{
				order.PaymentStatus = "approved";
				order.OrderStatus = "approved";
				order.OrderDate = DateTime.Now;
			}

			await this.orderService.UpdateOrder(order);

			await this.shoppingCartService.DeleteAllProteinsFromShoppingCart(userId);
			return this.RedirectToAction("OrderConfirmation", "ShoppingCart", new { id = order.Id });
		}

		public async Task<IActionResult> GetAllOrders()
		{
			var user = GetUserId();
			var model = await orderService.GetAllOrders(user);
			return View(model);
		}

		public IActionResult OrderConfirmation(int id)
		{
			return this.View(id);
		}
		private string GetUserId()
			=> this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
