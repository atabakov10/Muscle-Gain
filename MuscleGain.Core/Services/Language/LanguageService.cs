using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Language;
using MuscleGain.Infrastructure.Data.Common;

namespace MuscleGain.Core.Services.Language
{
	public class LanguageService : ILanguageService
	{
		private readonly IRepository repo;

		public LanguageService(IRepository repo)
		{
			this.repo = repo;
		}

		public async Task Add(LanguageViewModel languageModel)
		{
			var language = new Infrastructure.Data.Models.Protein.Language
			{
				Title = languageModel.Title,
			};

			await this.repo.AddAsync(language);
			await this.repo.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var language = await this.repo.GetByIdAsync<Infrastructure.Data.Models.Protein.Language>(id);

			if (language == null)
			{
				throw new Exception();
			}

			language.IsDeleted = true;

			await this.repo.SaveChangesAsync();
		}

		public async Task<IEnumerable<LanguageViewModel>> GetAll()
		{
			return await this.repo.AllReadonly<Infrastructure.Data.Models.Protein.Language>()
				.Where(l => l.IsDeleted == false)
				.Select(l => new LanguageViewModel
				{
					Id = l.Id,
					Title = l.Title
				}).ToListAsync();
		}

		public async Task<LanguageViewModel> GetLanguageForEdit(int id)
		{
			var model = await this.repo.GetByIdAsync<Infrastructure.Data.Models.Protein.Language>(id);

			if (model == null)
			{
				throw new Exception();
			}

			return new LanguageViewModel()
			{
				Id = model.Id,
				Title = model.Title,
			};
		}

		public async Task Update(LanguageViewModel model)
		{
			var language = await this.repo.GetByIdAsync<Infrastructure.Data.Models.Protein.Language>(model.Id);

			if (language == null)
			{
				throw new Exception();
			}

			language.Title = model.Title;

			await this.repo.SaveChangesAsync();
		}
	}
}
