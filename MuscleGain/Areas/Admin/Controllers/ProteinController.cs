using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;

namespace MuscleGain.Areas.Admin.Controllers
{
	public class ProteinController : BaseController
	{
		private readonly IProteinService proteinService;

		public ProteinController(IProteinService proteinService)
		{
			this.proteinService = proteinService;
		}

		public async Task<IActionResult> Index()
		{
			var allProteins = await this.proteinService.GetAllNotApproved();
			return this.View(allProteins);
		}

		public async Task<IActionResult> Details([FromRoute] int id)
		{
			var protein = await this.proteinService.GetForDetailsAsync(id);
			this.ViewData["Name"] = $"{protein.Name}";
			this.ViewData["Flavour"] = $"{protein.Flavour}";
			this.ViewData["Price"] = $"{protein.Price}";
			return this.View(protein);
		}

		[HttpPost]
		public async Task<IActionResult> Approve(int id)
		{
			try
			{
				await this.proteinService.ApproveProtein(id);

				this.TempData[MessageConstant.SuccessMessage] = "Protein is approved successfully";

				return this.RedirectToAction("Index", "Protein", new { area = "admin" });
			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = e.Message;
				return RedirectToAction("Index", "Home");
			}

		}

		[HttpPost]
		public async Task<IActionResult> Unapprove(int id)
		{
			try
			{
				await this.proteinService.UnapproveProtein(id);

				this.TempData[MessageConstant.SuccessMessage] = "Protein is unnaproved successfully";

				return this.RedirectToAction("Index", "Protein", new { area = "admin" });
			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = e.Message;
				return RedirectToAction("Index", "Home");
			}
		}
	}
}
