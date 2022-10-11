
using MuscleGain.Infrastructure.Data;
using MuscleGain.Models.Home;
using MuscleGain.Models.Proteins;
using MuscleGain.Services.Statistics;

namespace MuscleGain.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MuscleGain.Models;
    using System.Diagnostics;

    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly MuscleGainDbContext data;
        public HomeController(
            IStatisticsService statistics,
            MuscleGainDbContext data)
        { 
            this.data = data;
            this.statistics = statistics;
        }
        

        public IActionResult Index()
        {
            var proteins = this.data
                .Proteins
                .OrderByDescending(p => p.Id)
                .Select(p => new ProteinIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Grams = p.Grams,
                    Flavour = p.Flavour,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .Take(3)
                .ToList();

            var totalStatistics = this.statistics.Total();



            return View(new IndexViewModel
            {
                TotalProteins = totalStatistics.TotalProteins,
                TotalUsers = totalStatistics.TotalUsers,
                Proteins = proteins
            });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}