using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MuscleGain.Contracts;
using MuscleGain.Core.Constants;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Models.Users;
using MuscleGain.Services.User;

namespace MuscleGain.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MuscleGainDbContext data;
        private readonly IUserService service;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MuscleGainDbContext data,
            IUserService service)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this.data = data;
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
                FirstName = model.FirstName,
                EmailConfirmed = true,
                LastName = model.LastName,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager
                .AddClaimAsync(user, new Claim(ClaimTypeConstants.FirstName, user.FirstName ?? user.Email));

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
            await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Manager));
            await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Supervisor));
            await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));

            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> AddUsersToRoles()
        //{
        //    string email4 = "atabakov99@abv.bg";

        //    string email = "koko1926@abv.bg";

        //    var user4 = await _userManager.FindByNameAsync(email);
        //    await _userManager.AddToRolesAsync(user4, new string[] { RoleConstants.Supervisor, RoleConstants.Manager, RoleConstants.Administrator });

            

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

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            await this.service.UpdateUser(model);

            return RedirectToAction("MyProfile", "Account");
        }

    }
}
