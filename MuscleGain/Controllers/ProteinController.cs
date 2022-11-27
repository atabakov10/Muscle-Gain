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
			MuscleGainDbContext data,
			IUserService userService,
			ICategoryService categoryService)
		{
			this.proteinService = proteinService;
			//this.data = data;
			this.userService = userService;
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

			var proteinCategories = await this.proteinService.AllProteinCategoriesAsync();

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
		[Authorize(Roles = $"{RoleConstants.Manager}, {RoleConstants.Supervisor}")]
		public async Task<IActionResult> Add() => View(new AddProtein
		{
			Categories = await this.proteinService.GetProteinCategoriesAsync()
		});

		/// <summary>
		/// Checks your input data for add
		/// </summary>
		/// <returns>
		///	the model state needs to be valid and the product needs to get approved,
		/// otherwise the error will be given  
		/// </returns>

		[HttpPost]
		public async Task<IActionResult> Add(AddProtein protein)
		{
			int id = protein.CategoryId;

			if (!categoryService.CheckForCategory(id).IsFaulted)
			{
				this.ModelState.AddModelError(nameof(protein.CategoryId), "Category does not exist!");
			}

			if (!ModelState.IsValid)
			{
				TempData[MessageConstant.ErrorMessage] = "Something went wrong!";
				return View();
			}

			try
			{
				await proteinService.AddAsync(protein);

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
			TempData[MessageConstant.SuccessMessage] = "Successfully added protein";

			return RedirectToAction(nameof(All));
		}

		/// <summary>
		/// Gets the protein by id 
		/// </summary>
		/// <returns>
		///	a view with all the details about the protein which can be edited
		/// </returns>

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return new NotFoundResult();
			}

			var model = await proteinService.GetForEditAsync(id);

			return View(model);
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
			if (!ModelState.IsValid)
			{
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

			var data = await proteinService.GetForDetailsAsync(id);

			if (data == null)
			{
				return new NotFoundResult();
			}

			return View(data);
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
	}

}

