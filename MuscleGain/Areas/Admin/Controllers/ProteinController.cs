using Microsoft.AspNetCore.Cors.Infrastructure;
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
			var allCourses = await this.proteinService.GetAllNotApproved();
			return this.View(allCourses);
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
			await this.proteinService.ApproveProtein(id);
			this.TempData[MessageConstant.SuccessMessage] = "Protein approved successfully";
			return this.RedirectToAction("Index", "Protein", new { area = "admin" });
		}
	}
}
