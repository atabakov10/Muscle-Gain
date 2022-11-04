
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
    using MuscleGain.Services.Proteins;
    using System.Diagnostics;

    public class HomeController : BaseController
    {
        private readonly IStatisticsService statistics;
        private readonly MuscleGainDbContext data;
        private readonly IProteinService proteinService;
        public HomeController(
            IStatisticsService _statistics,
            MuscleGainDbContext _data,
            IProteinService _proteinService)
        { 
            data = _data;
            statistics = _statistics;
            proteinService = _proteinService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var proteins = await proteinService.LastThreeProteins();

            var totalStatistics = this.statistics.Total();



            return View(new IndexViewModel
            {
                TotalProteins = totalStatistics.TotalProteins,
                TotalUsers = totalStatistics.TotalUsers,
                Proteins = (List<ProteinIndexViewModel>)proteins
            });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}