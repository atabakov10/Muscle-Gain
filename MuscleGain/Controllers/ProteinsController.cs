using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models;
using MuscleGain.Models.Proteins;

namespace MuscleGain.Controllers
{
    public class ProteinsController : Controller
    {
        private readonly MuscleGainDbContext data;

        public ProteinsController(MuscleGainDbContext data)
        {
            this.data = data;
        }

        public async Task<IActionResult> All(string searchTerm)
        {
            var proteinsQuery = this.data.Proteins.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                proteinsQuery = proteinsQuery.Where(p => (p.Name + " " + p.Flavour).ToLower().Contains(searchTerm.ToLower())
                                                         || p.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var proteins = await this.data
                .Proteins
                .OrderByDescending(p => p.Id)
                .Select(p => new ProteinListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Flavour = p.Flavour,
                    Grams = p.Grams,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = p.ProteinCategory.Name
                })
                .ToListAsync();


            return View(new AllProteinsQueryModel
            {
                Proteins = proteins,
                SearchTerm = searchTerm
            });
        }
        public  IActionResult Add() => View(new AddProtein
        {
            Categories = this.GetProteinCategories()
        });


        [HttpPost]
        public async Task<IActionResult> Add(AddProtein protein)
        {
            if (!this.data.ProteinsCategories.Any(x => x.Id == protein.CategoryId))
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
            await this.data.Proteins.AddAsync(proteinToAdd);
            await this.data.SaveChangesAsync();

            return RedirectToAction("All", "Proteins");
        }

        private IEnumerable<ProteinCategoryViewModel> GetProteinCategories()
            => this.data
                .ProteinsCategories
                .Select(x => new ProteinCategoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
    }
 
}

