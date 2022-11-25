using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleGain.Core.Models.Language;

namespace MuscleGain.Core.Contracts
{
	public interface ILanguageService
	{
		Task Add(LanguageViewModel languageModel);

		Task<LanguageViewModel> GetLanguageForEdit(int id);

		Task Update(LanguageViewModel languageModel);

		Task Delete(int id);

		Task<IEnumerable<LanguageViewModel>> GetAll();
	}
}
