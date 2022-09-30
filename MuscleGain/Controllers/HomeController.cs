
using MuscleGain.Infrastructure.Data;
using MuscleGain.Models.Proteins;

namespace MuscleGain.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MuscleGain.Models;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly MuscleGainDbContext data;
        public HomeController(MuscleGainDbContext data) 
            => this.data = data;
        

        public IActionResult Index()
        {
            var protein = this.data
                .Proteins
                .OrderByDescending(p => p.Id)
                .Select(p => new ProteinListingViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Grams = p.Grams,
                    Flavour = p.Flavour,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Category = p.ProteinCategory.Name
                })
                .Take(3)
                .ToList();
            return View(protein);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}