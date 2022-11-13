using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Contracts;
using MuscleGain.Core.Constants;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Models.Proteins;
using MuscleGain.Models.Reviews;

namespace MuscleGain.Controllers
{
    public class ProteinsController : BaseController
    {
        private readonly IProteinService proteinService;
        private readonly MuscleGainDbContext data;
        private readonly IUserService userService;

        public ProteinsController(IProteinService proteinService,
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

        public async Task<IActionResult> Mine()
        {
            var model = new ProteinsQueryModel();

            return View(model);
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
                return View();
            }
            await proteinService.AddAsync(protein);

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

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var data = await proteinService.GetForDetailsAsync(id);

            return View(data);
        }

        public IActionResult CreateReview(int id)
        {
	        var userId = GetUserId();


			var model = new AddReviewViewModel()
	        {
		        UserId = userId,
		        ProteinId = id
	        };

	        return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewViewModel model)
        {
	        model.DateOfPublication = DateTime.Now;

	        //if (!ModelState.IsValid)
	        //{
		       // return View("CreateReview", model);
	        //}

	        await this.proteinService.AddReview(model);
	        TempData[MessageConstant.SuccessMessage] = "Successfully added review";

	        return RedirectToAction("Details", "Proteins", new { id = model.ProteinId });
        }

		[HttpPost, ActionName("Delete")]
        [Authorize(Policy = "CanDeleteProduct")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Protein protein = data.Proteins.Find(id);
            if (protein == null)
            {
                return new NotFoundResult();
            }

            if (id == null)
            {
                return new NotFoundResult();
            }
            data.Proteins.Remove(protein);
            await data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }
        private string GetUserId()
	        => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
	}

}

