using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models;
using MuscleGain.Models.Proteins;

namespace MuscleGain.Controllers
{
    [Authorize]
    public class ProteinsController : Controller
    {
        private readonly MuscleGainDbContext data;

        public ProteinsController(MuscleGainDbContext data)
        {
            this.data = data;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllProteinsQueryModel query)
        {
            var proteinsQuery =  this.data.Proteins.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Flavour))
            {
                proteinsQuery = proteinsQuery.Where(p => p.Flavour == query.Flavour);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                proteinsQuery = proteinsQuery.Where(p => (p.Name + " " + p.Flavour).ToLower().Contains(query.SearchTerm.ToLower())
                                                         || p.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }
            
            proteinsQuery = query.Sorting switch
            {
                ProteinSorting.DateCreated => proteinsQuery.OrderByDescending(p => p.Id),
                ProteinSorting.NameAndFlavour => proteinsQuery.OrderBy(p=>p.Name).ThenBy(p=> p.Flavour),
                _ => proteinsQuery.OrderByDescending(p=> p.Id)
            };

            var totalProteins = await proteinsQuery.CountAsync();

            var proteins = await proteinsQuery
                .Skip((query.CurrentPage-1)* AllProteinsQueryModel.ProteinsPerPage)
                .Take(AllProteinsQueryModel.ProteinsPerPage)
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

            var proteinsFlavours = await this.data
                .Proteins
                .Select(p => p.Flavour)
                .Distinct()
                .OrderBy(n=>n)
                .ToListAsync();

            query.TotalProteins = totalProteins;
            query.Flavours = proteinsFlavours;
            query.Proteins = proteins;

            return View(query);
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
            //TODO:Make protein price error when not ending with $!!!
            //if (!protein.Price.EndsWith("$"))
            //{
            // var Price = protein.Price ; 
            //    this.ModelState.AddModelError(Price, "Price should be in dollars!");
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

