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
        private readonly MuscleGainDbContext data;
        private readonly IUserService userService;

        public ProteinController(IProteinService proteinService,
	        MuscleGainDbContext data,
	        IUserService userService)
        {
            this.proteinService = proteinService;
            this.data = data;
            this.userService = userService;
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


        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.Manager}, {RoleConstants.Supervisor}")]
        public async Task<IActionResult> Add() => View(new AddProtein
        {
            Categories = await this.proteinService.GetProteinCategoriesAsync()
        });

        [HttpPost]
        public async Task<IActionResult> Add(AddProtein protein)
        {
            if (!this.data.ProteinsCategories.Any(x => x.Id == protein.CategoryId))
            {
                this.ModelState.AddModelError(nameof(protein.CategoryId), "Category does not exist!");
            }

            if (!ModelState.IsValid)
            {
	            TempData[MessageConstant.ErrorMessage] = "Something went wrong!";
                return View();
            }
            await proteinService.AddAsync(protein);
            TempData[MessageConstant.SuccessMessage] = "Successfully added protein";

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await proteinService.GetForEditAsync(id);

            return View(model);
        }

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var data = await proteinService.GetForDetailsAsync(id);

            return View(data);
        }


        [ActionName("Delete")]
        [Authorize(Policy = "CanDeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var protein = await data.Proteins.FindAsync(id);
            if (protein == null)
            {
                return new NotFoundResult();
            }

            if (id == null)
            {
                return new NotFoundResult();
            }
            data.Proteins.Remove(protein);
            TempData[MessageConstant.SuccessMessage] = "Successfully deleted the protein";
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        private string GetUserId()
	        => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}

}

