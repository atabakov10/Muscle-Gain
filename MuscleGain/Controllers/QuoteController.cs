using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Contracts;

namespace MuscleGain.Controllers
{
	public class QuoteController : BaseController
	{
		private readonly IQuotesService quotesService;

		public QuoteController(IQuotesService _quotesService)
		{
			quotesService = _quotesService;
		}
		public async Task<IActionResult> Index()
		{
			var quotesAll = await this.quotesService.GetAll();
			return this.View(quotesAll);
		}
	}
}
