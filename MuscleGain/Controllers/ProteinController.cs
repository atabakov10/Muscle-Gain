using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Proteins;

namespace MuscleGain.Controllers
{
	public class ProteinController : BaseController
	{
		private readonly IProteinService proteinService;

		private readonly ICategoryService categoryService;

		public ProteinController(
			IProteinService proteinService,
			ICategoryService categoryService)
		{
			this.proteinService = proteinService;
			this.categoryService = categoryService;
		}

		public async Task<IActionResult> All([FromQuery] ProteinsQueryModel query)
		{
			var queryResult = await this.proteinService.AllAsync(
				query.Category,
				query.SearchTerm,
				query.Sorting,
				query.CurrentPage,
				ProteinsQueryModel.ProteinsPerPage);

			var proteinCategories = await this.categoryService.AllProteinCategoriesAsync();

			query.Categories = proteinCategories;
			query.TotalProteins = queryResult.TotalProteins;
			query.Proteins = queryResult.Proteins;

			return View(query);
		}

		/// <summary>
		/// checks your authorization to add a protein
		/// </summary>
		/// <returns> the view to add a protein</returns>

		[HttpGet]
		[Authorize(Roles = RoleConstants.Seller)]
		public async Task<IActionResult> Add()
		{
			var user = this.GetUserId();

			return View(new AddProtein
			{
				Categories = await this.categoryService.GetAllCategories(),
				UserId = user
			});
		}

		/// <summary>
		/// Checks your input data for add
		/// </summary>
		/// <returns>
		///	the model state needs to be valid and the product needs to get approved,
		/// otherwise the error will be given  
		/// </returns>

		[HttpPost]
		public async Task<IActionResult> Add(AddProtein model)
		{
			var categories = await this.categoryService.GetAllCategories();
			if (!categories.Any(b => b.Id == model.CategoryId))
			{
				this.ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist");
			}

			if (!this.ModelState.IsValid)
			{
				model.Categories = categories;
				return this.View(model);
			}

			await this.proteinService.AddAsync(model);
			TempData[MessageConstant.ErrorMessage] =
				"The protein will be approved by the administrator and then will be published.";
			return this.RedirectToAction(nameof(this.All));
		}
		/// <summary>
		/// Gets the protein by id 
		/// </summary>
		/// <returns>
		///	a view with all the details about the protein which can be edited
		/// </returns>

		[HttpGet]
		[Authorize(Roles = RoleConstants.Seller)]
		public async Task<IActionResult> Edit(int id)
		{
			try
			{
				var model = await proteinService.GetForEditAsync(id);
				return View(model);
			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = "Oops, something went wrong...";
				return RedirectToAction("All", "Protein");
			}

		}

		/// <summary>
		/// Posts the view with the details of the protein
		/// </summary>
		/// <returns>
		///	the all view with the changes made on the protein
		/// </returns>

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

			TempData[MessageConstant.SuccessMessage] = "Successfully edited the protein!";

			return RedirectToAction(nameof(All));
		}

		/// <summary>
		/// Shows the details with the given id
		/// </summary>
		/// <returns>
		/// A view with the protein details
		/// </returns>

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			try
			{
				var data = await proteinService.GetForDetailsAsync(id);

				return View(data);
			}
			catch (ArgumentException ae)
			{
				TempData[MessageConstant.ErrorMessage] = ae.Message;
				return RedirectToAction("All", "Protein");
			}


		}

		/// <summary>
		/// Deletes the protein with the given id
		/// </summary>
		/// <returns>
		///	All other proteins
		/// </returns>

		[ActionName("Delete")]
		[Authorize(Policy = "CanDeleteProduct")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await proteinService.Delete(id);
				return RedirectToAction(nameof(All));

			}
			catch (Exception e)
			{
				TempData[MessageConstant.ErrorMessage] = e.Message;
				return RedirectToAction("All", "Protein");
			}

		}

		private string GetUserId()
			=> this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}

}


