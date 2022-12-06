
using Microsoft.AspNetCore.Authorization;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Home;

namespace MuscleGain.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MuscleGain.Models;
    using System.Diagnostics;

    public class HomeController : BaseController
    {
        private readonly IStatisticsService statistics;
        private readonly IProteinService proteinService;
        private readonly ILogger<HomeController> logger;
        public HomeController(
            IStatisticsService _statistics,
            IProteinService _proteinService,
            ILogger<HomeController> _logger)
        {
            statistics = _statistics;
            proteinService = _proteinService;
            logger = _logger;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var proteins = await proteinService.LastThreeProteins();

            var totalStatistics = this.statistics.Total();



            return View(new IndexViewModel
            {
                TotalProteins = totalStatistics.Result.TotalProteins,
                TotalUsers = totalStatistics.Result.TotalUsers,
                Proteins = (List<ProteinIndexViewModel>)proteins
            });
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error() =>  View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        
    }
}