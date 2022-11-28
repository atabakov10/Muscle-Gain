using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Quotes;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Services.Quote
{
    public class QuotesService : IQuotesService
    {
        private readonly IRepository repo;

        public QuotesService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task Add(QuoteViewModel quoteModel)
        {
	        var user = await this.repo.GetByIdAsync<ApplicationUser>(quoteModel.UserId);

	        if (user == null)
	        {
		        throw new Exception();
	        }
			var quote = new Infrastructure.Data.Models.Quotes.Quote
            {
                Text = quoteModel.Text,
                AuthorName = quoteModel.AuthorName,
                UserId = quoteModel.UserId,
            };

            await this.repo.AddAsync(quote);
            await this.repo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
	        var quote = await this.repo.GetByIdAsync<Infrastructure.Data.Models.Quotes.Quote>(id);

            if (quote == null)
            {
                throw new Exception();
            }

            quote.IsDeleted = true;

            await this.repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<QuoteViewModel>> GetAll()
        {
            return await this.repo.AllReadonly<Infrastructure.Data.Models.Quotes.Quote>()
                .Where(l => l.IsDeleted == false)
                .Select(l => new QuoteViewModel
                {
                    Id = l.Id,
                    Text = l.Text,
                    UserId = l.UserId,
                    AuthorName = l.AuthorName,
                }).ToListAsync();
        }

        public async Task<QuoteViewModel> GetQuoteForEdit(int id)
        {
            var model = await this.repo.GetByIdAsync<Infrastructure.Data.Models.Quotes.Quote>(id);

            if (model == null)
            {
                throw new Exception();
            }

            return new QuoteViewModel()
            {
                Id = model.Id,
                Text = model.Text,
                AuthorName = model.AuthorName,
            };
        }

        public async Task Update(QuoteViewModel model)
        {
            var quote = await this.repo.GetByIdAsync<Infrastructure.Data.Models.Quotes.Quote>(model.Id);

            if (quote == null)
            {
                throw new Exception();
            }

            quote.Text = model.Text;
            quote.AuthorName = model.AuthorName;

            await this.repo.SaveChangesAsync();
        }
    }
}
