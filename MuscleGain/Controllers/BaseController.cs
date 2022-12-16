using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MuscleGain.Core.Constants;

namespace MuscleGain.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public string UserProfilePicture
        {
			get
			{
				string picture = String.Empty;

				if (User?.Identity?.IsAuthenticated ?? false && User.HasClaim(c => c.Type == ClaimTypeConstants.ProfilePic))
				{
					picture = User.Claims
						.FirstOrDefault(c => c.Type == ClaimTypeConstants.ProfilePic)?
						.Value ?? picture;
				}

				return picture;
			}
        }
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
                ViewBag.UserProfilePicture = UserProfilePicture;
            }

            base.OnActionExecuted(context);
        }
    }
}
