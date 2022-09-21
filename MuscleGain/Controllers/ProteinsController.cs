using Microsoft.AspNetCore.Mvc;
using MuscleGain.Models.Proteins;

namespace MuscleGain.Controllers
{
    public class ProteinsController : Controller
    {
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(AddProtein protein)
        {
            return View();
        }
    }
}
