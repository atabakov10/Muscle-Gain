using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Category;
using MuscleGain.Infrastructure.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Categories
{
	public class CategoryService : ICategoryService
	{
		private readonly IRepository repo;
		private readonly ILogger logger;
		public CategoryService(IRepository repo,
			ILogger<CategoryService> logger)
		{
			this.repo = repo;
			this.logger = logger;
		}

		public async Task CreateCategory(ProteinCategoryViewModel model)
		{
			var category = new ProteinsCategories()
			{
				Name = model.Name,
			};

			await this.repo.AddAsync(category);
			await this.repo.SaveChangesAsync();
		}

		public async Task CheckForCategory(int categoryId)
		{

			var idExist = await repo.GetByIdAsync<ProteinsCategories>(categoryId);

			if (idExist == null)
			{
				logger.LogError($"Category Id - {categoryId} does not exist! ");
				throw new ApplicationException();
			}

		}

		public async Task Delete(int id)
		{
			var category = await this.repo.GetByIdAsync<ProteinsCategories>(id);
			if (category == null)
			{
				throw new Exception();
			}

			category.IsDeleted = true;
			await this.repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<ProteinCategoryViewModel>> GetAllCategories()
		{
			var categories = await this.repo.AllReadonly<ProteinsCategories>()
				.Where(c => c.IsDeleted == false)
				.Select(c => new ProteinCategoryViewModel()
				{
					Id = c.Id,
					Name = c.Name,
				}).ToListAsync();

			var categoriesToReturn = new List<ProteinCategoryViewModel>();
			foreach (var mainCategory in categories)
			{
				foreach (var proteinCategory in categories)
				{
					mainCategory.ProteinCategories.Add(proteinCategory);
				}

				categoriesToReturn.Add(mainCategory);
			}

			return categoriesToReturn;
		}

		public async Task<EditCategoryViewModel> GetCategoryForEdit(int id)
		{
			var model = await this.repo.GetByIdAsync<ProteinsCategories>(id);
			if (model == null)
			{
				throw new Exception();
			}

			return new EditCategoryViewModel()
			{
				Id = model.Id,
				Name = model.Name,
			};
		}

		public async Task Update(EditCategoryViewModel model)
		{
			var category = await this.repo.GetByIdAsync<ProteinsCategories>(model.Id);
			if (category == null)
			{
				throw new Exception();
			}

			category.Name = model.Name;
			await this.repo.SaveChangesAsync();
		}
	}
}
