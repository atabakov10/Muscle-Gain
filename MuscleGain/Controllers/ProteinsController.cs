using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Contracts;
using MuscleGain.Core.Constants;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Models.Api.Proteins;
using MuscleGain.Models.Proteins;
using MuscleGain.Services.Proteins;
using System.Net;

namespace MuscleGain.Controllers
{
    public class ProteinsController : BaseController
    {
        private readonly IProteinService proteinService;
        private readonly MuscleGainDbContext data;

        public ProteinsController(IProteinService proteinService, MuscleGainDbContext data)
        {
            this.proteinService = proteinService;
            this.data = data;
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
        public async Task<IActionResult> Details(int id)
        {
            var data = await proteinService.GetForDetailsAsync(id);

            return View(data);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "CanDeleteProduct")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Protein protein =  data.Proteins.Find(id);
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

    }

}

