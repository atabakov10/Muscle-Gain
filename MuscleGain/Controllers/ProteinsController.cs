using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Add() => View(new AddProtein
        {
            Categories = this.GetProteinCategories()
        });

        [HttpPost]
        public IActionResult Add(AddProtein protein)
        {
            if (!this.data.ProteinsCategories.Any(x=> x.Id == protein.CategoryId))
            {
                this.ModelState.AddModelError(nameof(protein.CategoryId), "Category does not exist!");
            }
                //if (!ModelState.IsValid)
                //{
                //   protein.Categories = this.GetProteinCategories();
                //    return View(protein);
                //}

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
            this.data.Proteins.Add(proteinToAdd);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
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
