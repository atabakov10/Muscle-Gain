using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;

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
