﻿using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Language;

namespace MuscleGain.Areas.Admin.Controllers
{
	public class LanguageController : BaseController
	{
		private readonly ILanguageService languageService;

		public LanguageController(ILanguageService languageService)
		{
			this.languageService = languageService;
		}

		public async Task<IActionResult> Index()
		{
			var languages = await this.languageService.GetAll();
			return this.View(languages);
		}

		public IActionResult Add()
		{
			var model = new LanguageViewModel();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(LanguageViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View(model);
			}

			await this.languageService.Add(model);
			return this.RedirectToAction(nameof(this.Index));
		}

		public async Task<IActionResult> Edit(int id)
		{
			var model = await this.languageService.GetLanguageForEdit(id);
			return this.View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(LanguageViewModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return this.View("Edit", model);
			}

			await this.languageService.Update(model);
			return this.RedirectToAction("Index", "Language");
		}

		[HttpPost]
		public async Task<IActionResult> Delete([FromForm] int id)
		{
			await this.languageService.Delete(id);
			return this.RedirectToAction("Index", "Language");
		}
	}
}
