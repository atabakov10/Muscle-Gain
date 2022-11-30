using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Areas.Admin.Controllers;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Quotes;
using MuscleGain.Core.Models.Reviews;
using System.Security.Claims;

namespace MuscleGain.Areas.Author.Controllers
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
		public async Task<IActionResult> Add(QuoteViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			await this._quotesService.Add(model);
			return this.RedirectToAction(nameof(this.Index));
		}
		
		public async Task<IActionResult> Edit(int id)
		{
			var userId = this.GetUserId();

			var model = await this._quotesService.GetQuoteForEdit(id, userId);
			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(QuoteViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View("Edit", model);
			}

			await this._quotesService.Update(model);
			return this.RedirectToAction("Index", "Quote");
		}
		private string GetUserId()
			=> this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
