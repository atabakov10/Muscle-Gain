using Microsoft.AspNetCore.Authorization;
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

			var model = await this._quotesService.GetQuoteForEdit(id, user);
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

		[HttpPost]
		public async Task<IActionResult> Delete([FromForm] int id)
		{
			await this._quotesService.Delete(id);
			return this.RedirectToAction("Index", "Quote");
		}
		private string GetUserId()
			=> this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
