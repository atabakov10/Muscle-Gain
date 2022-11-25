using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using System.Security.Claims;

namespace MuscleGain.Controllers
{
    public class ShoppingCartController : BaseController
    {

        private readonly IShoppingCartService shoppingCartService;
        //private readonly IOrderService orderService;
        private readonly IProteinService proteinService;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            //IOrderService orderService,
            IProteinService proteinService)
        {
            this.shoppingCartService = shoppingCartService;
            //this.orderService = orderService;
            this.proteinService = proteinService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var shoppingCart = await this.shoppingCartService.GetShoppingCart(userId);

            return this.View(shoppingCart);
        }

        public async Task<IActionResult> Add([FromRoute] int id)
        {
            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            try
            {
                await this.shoppingCartService.AddProteinInShoppingCart(id, userId);
            }
            catch (Exception ex)
            {
                this.TempData[MessageConstant.ErrorMessage] = ex.Message;
                return this.RedirectToAction("All", "Protein");
            }

            this.TempData[MessageConstant.SuccessMessage] = "Successfully added protein to Shopping cart";

            return this.RedirectToAction("All", "Protein");
        }

        public async Task<IActionResult> Delete(int id)
        {

            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await this.shoppingCartService.DeleteProteinFromShoppingCart(id, userId);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> DeleteAll()
        {
            var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            await this.shoppingCartService.DeleteAllProteinsFromShoppingCart(userId);

            return this.RedirectToAction(nameof(this.Index));
        }

        //public async Task<IActionResult> Summary()
        //{
        //    var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    var shoppingCart = await this.shoppingCartService.GetShoppingCart(userId);

        //    return this.View(shoppingCart);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Summary(string stripeToken)
        //{
        //    var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //    var shoppingCart = await this.shoppingCartService.GetShoppingCart(userId);
        //    var order = new OrderViewModel()
        //    {
        //        Courses = shoppingCart.Courses,
        //        TotalPrice = shoppingCart.TotalPrice,
        //        OrderDate = DateTime.Now,
        //        OrderStatus = "pending",
        //        PaymentStatus = "pending",
        //    };

        //    order.Id = await this.orderService.AddOrder(order, userId);
        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = Convert.ToInt32(order.TotalPrice * 100),
        //        Currency = "bgn",
        //        Description = "Order ID : " + order.Id,
        //        Source = stripeToken,
        //    };

        //    var service = new ChargeService();
        //    Charge charge = service.Create(options);
        //    if (charge.Id == null)
        //    {
        //        order.PaymentStatus = "rejected";
        //    }
        //    else
        //    {
        //        order.TransactionId = charge.Id;
        //    }

        //    if (charge.Status.ToLower() == "succeeded")
        //    {
        //        order.PaymentStatus = "approved";
        //        order.OrderStatus = "approved";
        //        order.OrderDate = DateTime.Now;
        //    }

        //    await this.orderService.UpdateOrder(order);
        //    foreach (var course in shoppingCart.Courses)
        //    {
        //        await this.proteinService.AddStudentToCourse(course.Id, userId);
        //    }

        //    await this.shoppingCartService.DeleteAllCoursesFromShoppingCart(userId);
        //    return this.RedirectToAction("OrderConfirmation", "ShoppingCart", new { id = order.Id });
        //}

        //public IActionResult OrderConfirmation(int id)
        //{
        //    return this.View(id);
        //}
    }
}
