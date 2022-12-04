using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Quotes;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Core.Services.Quote
{
	public class QuotesService : IQuotesService
	{
		private readonly MuscleGainDbContext dbContext;

		public QuotesService(MuscleGainDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task Add(AddQuoteViewModel quoteModel)
		{
			var user = await this.dbContext.FindAsync<ApplicationUser>(quoteModel.UserId);

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

			await this.dbContext.AddAsync(quote);
			await this.dbContext.SaveChangesAsync();
		}

		public async Task Delete(int id)
		{
			var quote = await this.dbContext.FindAsync<Infrastructure.Data.Models.Quotes.Quote>(id);

			if (quote == null)
			{
				throw new Exception();
			}

			quote.IsDeleted = true;

			await this.dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<QuoteViewModel>> GetAll()
		{
			return await this.dbContext.Quotes
				.Where(q => q.IsDeleted == false)
				.Include(x => x.User)
				.Select(q => new QuoteViewModel
				{
					Id = q.Id,
					Text = q.Text,
					UserId = q.UserId,
					PublisherFullName = $"{q.User.FirstName} {q.User.LastName}",
					AuthorName = q.AuthorName,
				}).ToListAsync();
		}

		public async Task<AddQuoteViewModel> GetQuoteForEdit(int id, string userId)
        {
            var user = await this.dbContext.FindAsync<ApplicationUser>(userId);

			var model = await this.dbContext.FindAsync<Infrastructure.Data.Models.Quotes.Quote>(id);

			if (model == null)
			{
				throw new Exception();
			}

			return new AddQuoteViewModel()
			{
				Id = model.Id,
				Text = model.Text,
				AuthorName = model.AuthorName,
				UserId = user.Id,
			};
		}

		public async Task Update(AddQuoteViewModel model)
		{
			var quote = await this.dbContext.FindAsync<Infrastructure.Data.Models.Quotes.Quote>(model.Id);

			if (quote == null)
			{
				throw new Exception();
			}
			quote.Id = model.Id;
            quote.Text = model.Text;
			quote.AuthorName = model.AuthorName;

			await this.dbContext.SaveChangesAsync();
		}
	}
}
