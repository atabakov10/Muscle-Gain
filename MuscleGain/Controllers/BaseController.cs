using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MuscleGain.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
