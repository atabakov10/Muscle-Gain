using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MuscleGain.Core.Constants;
using System.Data;

namespace MuscleGain.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleConstants.Supervisor)]
    [Area("Admin")]
    public class BaseController : Controller
    {
        public string UserFirstName
        {
            get
            {
                string firstName = string.Empty;

                if (User?.Identity?.IsAuthenticated ?? false && User.HasClaim(c => c.Type == ClaimTypeConstants.FirstName))
                {
                    firstName = User.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypeConstants.FirstName)?
                        .Value ?? firstName;
                }

                return firstName;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                ViewBag.UserFirstName = UserFirstName;
            }

            base.OnActionExecuted(context);
        }
    }
}
