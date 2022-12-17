using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Quotes;
using System.Security.Claims;

namespace MuscleGain.Areas.Admin.Controllers
{
	public class QuoteController : BaseController
	{
		private readonly IQuotesService _quotesService;

		public QuoteController(IQuotesService quotesService)
		{
			this._quotesService = quotesService;
		}

		public async Task<IActionResult> Index()
		{
			var quotesAll = await this._quotesService.GetAll();
			return this.View(quotesAll);
		}


		public async Task<IActionResult> Edit(int id)
		{
			var user = this.GetUserId();
			try
			{
				var model = await this._quotesService.GetQuoteForEdit(id, user);
				return this.View(model);
			}
			catch (Exception)
			{
				TempData[MessageConstant.ErrorMessage] = "The quote doesn't exist!";
				return RedirectToAction("Index", "Quote");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Update(AddQuoteViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View("Edit", model);
			}

			await this._quotesService.Update(model);
			return this.RedirectToAction("Index", "Quote");
		}

		[HttpPost]
		public async Task<IActionResult> Delete([FromForm] int id)
		{
			try
			{
				await this._quotesService.Delete(id);
				return this.RedirectToAction("Index", "Quote");
			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = e.Message;
				return RedirectToAction("Index", "User");
			}
		}
		
		public IActionResult Add()
		{

			var userId = this.GetUserId();
			
			var model = new QuoteViewModel()
			{
				UserId = userId
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddQuoteViewModel model)
		{

			if (!this.ModelState.IsValid)
			{
				return this.View();
			}

			await this._quotesService.Add(model);
			return this.RedirectToAction(nameof(this.Index));
		}

		
		private string GetUserId()
			=> this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
