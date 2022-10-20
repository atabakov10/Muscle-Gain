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
using System.Net;

namespace MuscleGain.Controllers
{
    public class ProteinsController : BaseController
    {
        private readonly IProteinService _proteins;
        private readonly MuscleGainDbContext _data;

        public ProteinsController(IProteinService proteins, MuscleGainDbContext data)
        {
            this._proteins = proteins;
            this._data = data;
        }

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

        [HttpGet]
        [Authorize(Roles = $"{RoleConstants.Manager}, {RoleConstants.Supervisor}")]
        public IActionResult Add() => View(new AddProtein
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

            if (!ModelState.IsValid)
            {
                return View();
            }

            await _proteins.Add(protein);

            return RedirectToAction(nameof(All));
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            Protein protein = _data.Proteins.Find(id);
            if (protein == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(protein);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Policy = "CanDeleteProduct")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Protein protein = _data.Proteins.Find(id);
            _data.Proteins.Remove(protein);
            await _data.SaveChangesAsync();
            return RedirectToAction(nameof(All));
        }

        //[HttpPost]
        //[Authorize(Policy = "CanDeleteProduct")]
        //public async Task<IActionResult> Delete([FromForm] string id)
        //{
        //    int idGuid = int.Parse(id);
        //    await _proteins.Delete(idGuid);

        //    return RedirectToAction(nameof(All));
        //}
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

