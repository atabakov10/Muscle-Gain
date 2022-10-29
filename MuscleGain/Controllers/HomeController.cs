
using Microsoft.AspNetCore.Authorization;
using MuscleGain.Core.Constants;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Models.Home;
using MuscleGain.Models.Proteins;

namespace MuscleGain.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MuscleGain.Contracts;
    using MuscleGain.Models;
    using System.Diagnostics;

    public class HomeController : BaseController
    {
        private readonly IStatisticsService _statistics;
        private readonly MuscleGainDbContext _data;
        public HomeController(
            IStatisticsService statistics,
            MuscleGainDbContext data)
        { 
            this._data = data;
            this._statistics = statistics;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var proteins = this._data
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

            var totalStatistics = this._statistics.Total();



            return View(new IndexViewModel
            {
                TotalProteins = totalStatistics.TotalProteins,
                TotalUsers = totalStatistics.TotalUsers,
                Proteins = proteins
            });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}