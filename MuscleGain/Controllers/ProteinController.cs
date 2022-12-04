using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Models.Reviews;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Controllers
{
	public class ProteinController : BaseController
	{
		private readonly IProteinService proteinService;

		//private readonly MuscleGainDbContext data;
		private readonly IUserService userService;
		private readonly ICategoryService categoryService;
		private readonly ILogger logger;

		public ProteinController(
			IProteinService proteinService,
			IUserService userService,
			ICategoryService categoryService,
			ILogger<ProteinController> logger)
		{
			this.proteinService = proteinService;
			this.userService = userService;
			this.categoryService = categoryService;
			this.logger = logger;
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

			var currentUserId = this.GetUserId();

			//model.UserId = currentUserId;

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


			if (id == null)
			{
				return new NotFoundResult();
			}

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
			if (id == null)
			{
				return new NotFoundResult();
			}


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
			if (id == null)
			{
				return new NotFoundResult();
			}

			await proteinService.Delete(id);

			return RedirectToAction(nameof(All));
		}

		private string GetUserId()
			=> this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}

}


