using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Models.Api.Proteins;
using MuscleGain.Models.Proteins;
using MuscleGain.Services.Proteins;

namespace MuscleGain.Controllers
{
    [Authorize]
    public class ProteinsController : Controller
    {
        private readonly IProteinService _proteins;
        private readonly MuscleGainDbContext _data;

        public ProteinsController(IProteinService proteins, MuscleGainDbContext data)
        {
            this._proteins = proteins;
            this._data = data;
        }
        
 

        [AllowAnonymous]
        public async Task<IActionResult>  All([FromQuery]AllProteinsQueryModel query)
        {
            var queryResult = await this._proteins.All(
                query.Flavour,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllProteinsQueryModel.ProteinsPerPage);

            var proteinsFlavours = await this._proteins.AllProteinFlavours();

            query.Flavours = proteinsFlavours;
            query.TotalProteins = queryResult.TotalProteins;
            query.Proteins = queryResult.Proteins;

            return View(query);
        }
        public  IActionResult Add() => View(new AddProtein
        {
            Categories = this.GetProteinCategories()
        });


        [HttpPost]
        public async Task<IActionResult> Add(AddProtein protein)
        {
            if (!this._data.ProteinsCategories.Any(x => x.Id == protein.CategoryId))
            {
                this.ModelState.AddModelError(nameof(protein.CategoryId), "Category does not exist!");
            }

            var proteinToAdd = new Protein
            {
                Name = protein.Name,
                Grams = protein.Grams,
                Flavour = protein.Flavour,
                Price = protein.Price,
                Description = protein.Description,
                ImageUrl = protein.ImageUrl,
                CategoryId = protein.CategoryId

            };
            await this._data.Proteins.AddAsync(proteinToAdd);
            await this._data.SaveChangesAsync();

            return RedirectToAction("All", "Proteins");
        }

        private IEnumerable<ProteinCategoryViewModel> GetProteinCategories()
            => this._data
                .ProteinsCategories
                .Select(x => new ProteinCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
    }
 
}

