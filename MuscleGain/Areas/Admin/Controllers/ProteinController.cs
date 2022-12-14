using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Proteins;

namespace MuscleGain.Areas.Admin.Controllers
{
	public class ProteinController : BaseController
	{
		private readonly IProteinService proteinService;
		private readonly ICategoryService categoryService;

		public ProteinController(IProteinService proteinService,
			ICategoryService categoryService)
		{
			this.proteinService = proteinService;
			this.categoryService = categoryService;
		}

		public async Task<IActionResult> Index()
		{
			return this.View();
		}

		public async Task<IActionResult> Details([FromRoute] int id)
		{
			var protein = await this.proteinService.GetForDetailsAsync(id);
			this.ViewData["Name"] = $"{protein.Name}";
			this.ViewData["Flavour"] = $"{protein.Flavour}";
			this.ViewData["Price"] = $"{protein.Price}";
			return this.View(protein);
		}

		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await proteinService.Delete(id);
				return RedirectToAction(nameof(AllProteins));

			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = e.Message;
				return RedirectToAction("All", "Protein");
			}
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var model = await proteinService.GetForEditAsync(id);
				return View(model);
			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = e.Message;
				return RedirectToAction(nameof(AllProteins));
			}

		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditProteinViewModel model)
		{
			var categories = await this.categoryService.GetAllCategories();


			if (!ModelState.IsValid)
			{
				model.Categories = categories;
				return View(model);
			}

			await proteinService.EditAsync(model);

			return RedirectToAction(nameof(AllProteins));
		}
		//TODO: Add protein method.
		public async Task<IActionResult> NotApprovedProteins()
		{
			try
			{
				var allProteins = await this.proteinService.GetAllNotApproved();
				return this.View(allProteins);

			}
			catch (NullReferenceException ne)
			{
				TempData[MessageConstant.ErrorMessage] = ne.Message;
				return RedirectToAction("Index", "Protein");
			}

		}

	
		public async Task<IActionResult> AllProteins()
		{
			try
			{
				var allProteins = await this.proteinService.GetAllProteins();
				return this.View(allProteins);

			}
			catch (NullReferenceException ne)
			{
				TempData[MessageConstant.ErrorMessage] = ne.Message;
				return RedirectToAction("Index", "Protein");
			}

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
