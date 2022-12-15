using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Users;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Controllers
{
	public class AccountController : BaseController
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUserService service;
		private readonly ICloudinaryService cloudinaryService;

		private readonly string defaultPicture = "file:///C:/Users/HP/Desktop/6a5b3185c490202f2b693763a1f98abf.jpg";


		public AccountController(
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IUserService service,
			ICloudinaryService cloudinaryService)
		{
			this._userManager = userManager;
			this._signInManager = signInManager;
			this._roleManager = roleManager;
			this.service = service;
			this.cloudinaryService = cloudinaryService;
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			var model = new RegisterViewModel();

			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string imageUrl = this.defaultPicture;

			if (model.ImageUrl != null)
			{
				imageUrl = await this.cloudinaryService.UploudAsync(model.ImageUrl);
			}


			var user = new ApplicationUser()
			{
				Email = model.Email,
				ImageUrl = imageUrl,
				FirstName = model.FirstName,
				EmailConfirmed = true,
				LastName = model.LastName,
				UserName = model.Email,
				PhoneNumber = model.PhoneNumber
			};

			var result = await _userManager.CreateAsync(user, model.Password);

			await _userManager
				.AddClaimAsync(user, new Claim(ClaimTypeConstants.FirstName, user.FirstName ?? user.Email));

			await _userManager
				.AddClaimAsync(user, new Claim(ClaimTypeConstants.ProfilePic, user.ImageUrl ?? String.Empty));
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, isPersistent: false);

				TempData[MessageConstant.SuccessMessage] = "Your register was successful!";

				return RedirectToAction("Index", "Home");
			}

			foreach (var item in result.Errors)
			{
				ModelState.AddModelError("", item.Description);
			}

			TempData[MessageConstant.ErrorMessage] = "Your register is invalid!";
			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Login(string? returnUrl = null)
		{
			var model = new LoginViewModel()
			{
				ReturnUrl = returnUrl,
			};

			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			TempData[MessageConstant.SuccessMessage] = "You've logged in successfully!";
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user != null)
			{
				var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

				if (result.Succeeded)
				{

					if (model.ReturnUrl != null)
					{
						return Redirect(model.ReturnUrl);
					}

					return RedirectToAction("Index", "Home");
				}
			}

			ModelState.AddModelError("", "Invalid login");

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> CreateRoles()
		{
			await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));
			await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Author));
			await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Seller));

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> AddUsersToRoles()
		{
			string email = "atabakov99@abv.bg";
			var user4 = await _userManager.FindByNameAsync(email);
			var result = await _userManager.AddToRoleAsync(user4, RoleConstants.Administrator);

			if (result.Succeeded)
			{
				TempData[MessageConstant.SuccessMessage] = "Successfully added role to the user!";
			}
			else
			{
				TempData[MessageConstant.ErrorMessage] = "Something went wrong...";
			}

			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> MyProfile()
		{
			var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			try
			{
				var model = await this.service.GetUserProfile(userId);
				return View(model);
			}
			catch (NullReferenceException ne)
			{
				TempData[MessageConstant.ErrorMessage] = ne.Message;
				return RedirectToAction("Index", "Home");
			}

		}

		public async Task<IActionResult> Edit()
		{
			var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			try
			{
				var model = await this.service.GetUserForEdit(userId);

				return View(model);

			}
			catch (NullReferenceException ne)
			{
				TempData[MessageConstant.ErrorMessage] = ne.Message;
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(UserEditViewModel model)
		{
			var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
			
			model.Id = userId;
			try
			{
				await this.service.UpdateUser(model);

				TempData[MessageConstant.SuccessMessage] = "Successfully updated your profile!";

				return RedirectToAction("MyProfile", "Account");
			}
			catch (NullReferenceException ne)
			{
				TempData[MessageConstant.ErrorMessage] = ne.Message;
				return RedirectToAction("Index", "User");
			}
		}

		public IActionResult AccessDenied()
		{
			return this.View();
		}

	}
}
