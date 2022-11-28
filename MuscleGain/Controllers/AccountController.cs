﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Constants;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Users;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService service;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserService service)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this.service = service;
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


            var user = new ApplicationUser()
            {
                Email = model.Email,
                ImageUrl = model.ImageUrl,
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

        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Manager));
        //    await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Supervisor));
        //    await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));
        //    await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Author));

        //    return RedirectToAction("Index", "Home");
        //}

        //public async Task<IActionResult> AddUsersToRoles()
        //{
        //    //string email4 = "atabakov99@abv.bg";

        //    string email = "v_tosheva02@abv.bg";

        //    var user4 = await _userManager.FindByNameAsync(email);
        //    var result = await _userManager.AddToRolesAsync(user4, new string[] { RoleConstants.Supervisor, RoleConstants.Manager, RoleConstants.Administrator });

        //    if (result.Succeeded)
        //    {
        //        TempData[MessageConstant.SuccessMessage] = "Successfully added role to the user!";
        //    }
        //    else
        //    {
        //        TempData[MessageConstant.ErrorMessage] = "Something went wrong...";
        //    }

        //    return RedirectToAction("Index", "Home");
        //}
        public async Task<IActionResult> MyProfile()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await this.service.GetUserProfile(userId);

            return View(model);
        }

        public async Task<IActionResult> Edit()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await this.service.GetUserForEdit(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            model.Id = userId;

            await this.service.UpdateUser(model);


			return RedirectToAction("MyProfile", "Account");
        }

        public IActionResult AccessDenied()
        {
            return this.View();
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string? returnurl = null)
        //{
	       // returnurl = returnurl ?? this.Url.Content("~/");
	       // if (this.ModelState.IsValid)
	       // {
		      //  //get the info about the user from external login provider
		      //  var info = await this._signInManager.GetExternalLoginInfoAsync();
		      //  if (info == null)
		      //  {
			     //   return this.View("Error");
		      //  }

		      //  var user = new ApplicationUser()
		      //  {
			     //   UserName = model.UserName,
			     //   Email = model.Email,
			     //   FirstName = model.FirstName,
			     //   LastName = model.LastName,
		      //  };

		      //  var result = await this._userManager.CreateAsync(user);
		      //  if (result.Succeeded)
		      //  {
			     //   result = await this._userManager.AddLoginAsync(user, info);
			     //   if (result.Succeeded)
			     //   {
				    //    await this._signInManager.SignInAsync(user, isPersistent: false);
				    //    await this._signInManager.UpdateExternalAuthenticationTokensAsync(info);
				    //    return this.LocalRedirect(returnurl);
			     //   }
		      //  }

		      //  this.ModelState.AddModelError("Email", "There was an error");
	       // }

	       // this.ViewData["ReturnUrl"] = returnurl;
	       // return this.View(model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ExternalLoginCallback(string returnurl = null, string remoteError = null)
        //{
	       // if (remoteError != null)
	       // {
		      //  this.ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
		      //  return this.View(nameof(this.Login));
	       // }

	       // var info = await this._signInManager.GetExternalLoginInfoAsync();
	       // if (info == null)
	       // {
		      //  return this.RedirectToAction(nameof(this.Login));
	       // }

	       // //Sign in the user with this external login provider, if the user already has a login.
	       // var result = await this._signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
	       // if (result.Succeeded)
	       // {
		      //  //update any authentication tokens
		      //  await this._signInManager.UpdateExternalAuthenticationTokensAsync(info);
		      //  return this.LocalRedirect(returnurl);
	       // }
	       // else
	       // {
		      //  //If the user does not have account, then we will ask the user to create an account.
		      //  this.ViewData["ReturnUrl"] = returnurl;
		      //  this.ViewData["ProviderDisplayName"] = info.ProviderDisplayName;
		      //  var email = info.Principal.FindFirstValue(ClaimTypes.Email);
		      //  return this.View("ExternalLoginConfirmation", new ExternalLoginViewModel { Email = email });
	       // }

        //}
        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult ExternalLogin(string provider, string returnurl = null)
        //{
	       // returnurl = returnurl ?? this.Url.Content("~/");
	       // //request a redirect to the external login provider
	       // var redirecturl = this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnurl });
	       // var properties = this._signInManager.ConfigureExternalAuthenticationProperties(provider, redirecturl);
	       // return this.Challenge(properties, provider);
        //}
	}
}
