using MuscleGain.Core.Models.Quotes;

namespace MuscleGain.Core.Contracts
{
	public interface IQuotesService
	{
		Task Add(AddQuoteViewModel quoteModel);

		Task<AddQuoteViewModel> GetQuoteForEdit(int id, string userId);

		Task Update(AddQuoteViewModel quoteModel);

		Task Delete(int id);

		Task<IEnumerable<QuoteViewModel>> GetAll();
	}
}
