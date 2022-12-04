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
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Categories
{
	public class CategoryService : ICategoryService
	{
		private readonly MuscleGainDbContext dbContext;

		public CategoryService(MuscleGainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task CreateCategory(ProteinCategoryViewModel model)
		{
			var category = new ProteinsCategories()
			{
				Name = model.Name,
            };

			await this.dbContext.AddAsync(category);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task CheckForCategory(int categoryId)
		{

			var idExist = await dbContext.FindAsync<ProteinsCategories>(categoryId);

			if (idExist == null)
			{
				throw new NullReferenceException();
			}

		}

		public async Task Delete(int id)
        {
            var category = await this.dbContext.FindAsync<ProteinsCategories>(id);
			if (category == null)
			{
				throw new NullReferenceException();
			}

			category.IsDeleted = true;
			await this.dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<ProteinCategoryViewModel>> GetAllCategories()
		{
			var categories = await this.dbContext.ProteinsCategories
				.Where(c => c.IsDeleted == false)
				.Select(c => new ProteinCategoryViewModel()
				{
					Id = c.Id,
					Name = c.Name,
                }).ToListAsync();

			return categories;
		}
		public async Task<IEnumerable<string>> AllProteinCategoriesAsync()
			=> await dbContext.ProteinsCategories
				.Where(x => x.IsDeleted == false)
				.Select(p => p.Name)
				.Distinct()
				.OrderBy(n => n)
				.ToListAsync();

		public async Task<EditCategoryViewModel> GetCategoryForEdit(int id)
		{
			var model = await this.dbContext.FindAsync<ProteinsCategories>(id);
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
			var category = await this.dbContext.FindAsync<ProteinsCategories>(model.Id);
			if (category == null)
			{
				throw new Exception();
			}

			category.Name = model.Name;
			await this.dbContext.SaveChangesAsync();
		}
	}
}
