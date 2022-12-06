using Microsoft.AspNetCore.Mvc;

namespace MuscleGain.Areas.Admin.Controllers
{
	public class HomeController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
