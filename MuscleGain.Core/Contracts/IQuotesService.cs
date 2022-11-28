using MuscleGain.Core.Models.Quotes;

namespace MuscleGain.Core.Contracts
{
	public interface IQuotesService
	{
		Task Add(QuoteViewModel quoteModel);

		Task<QuoteViewModel> GetQuoteForEdit(int id);

		Task Update(QuoteViewModel quoteModel);

		Task Delete(int id);

		Task<IEnumerable<QuoteViewModel>> GetAll();
	}
}
