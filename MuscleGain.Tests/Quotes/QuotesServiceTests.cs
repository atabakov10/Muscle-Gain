using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Services.Quote;
using MuscleGain.Infrastructure.Data.Models.Quotes;
using MuscleGain.Core.Models.Quotes;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Tests.Quotes
{
	[TestFixture]
	public class QuotesServiceTests
	{
		[Test]
		public async Task AddShouldReturnAddQuote()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var quotesService = new QuotesService(dbContext);

			var user = new ApplicationUser()
			{
				Id = "tabaka10",
				FirstName = "Angel",
				LastName = "Tabakov",
			};



			var firstQuote = new AddQuoteViewModel()
			{
				Id = 1,
				Text = "The best quote ever...",
				UserId = "tabaka10",
				AuthorName = "Tabaka"
			};
			dbContext.Users.Add(user);
			await quotesService.Add(firstQuote);

			var result = await dbContext.Quotes.CountAsync();

			Assert.That(result, Is.EqualTo(1));
		}


		[Test]
		public async Task GetAllShouldReturnAllCategories()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var quotesService = new QuotesService(dbContext);

			var user = new ApplicationUser()
			{
				Id = "a10tabakov",
				FirstName = "Angel",
				LastName = "Tabakov"
			};

			var firstQuote = new Quote
			{
				Id = 1,
				Text = "testContent",
				UserId = user.Id,
			};

			var secondQuote = new Quote
			{
				Id = 2,
				Text = "testContent",
				UserId = user.Id,
			};


			dbContext.Quotes.Add(firstQuote);
			dbContext.Quotes.Add(secondQuote);
			await dbContext.SaveChangesAsync();

			var result = await quotesService.GetAll();

			Assert.NotNull(result.Count());
			Assert.AreEqual("testContent", dbContext.Quotes.FirstOrDefaultAsync().Result.Text);
			Assert.AreEqual("a10tabakov", dbContext.Quotes.FirstOrDefaultAsync().Result.UserId);
		}
		[Test]
		public async Task DeleteShouldDeleteQuoteById()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var quotesService = new QuotesService(dbContext);

			var quote = new Quote()
			{
				Id = 4,
				UserId = "tabaka10",
				Text = "abcdefg",
				User = new ApplicationUser
				{
					Id = "tabaka10",
					UserName = "Icaka99",
				},
			};

			await dbContext.Quotes.AddAsync(quote);
			await dbContext.SaveChangesAsync();

			await quotesService.Delete(4);

			Assert.False(dbContext.Quotes.Any(x => x.IsDeleted == false));
		}

		[Test]
		public async Task UpdateShouldUpdateCorrectQuoteById()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var quotesService = new QuotesService(dbContext);

			var firstQuote = new Quote()
			{
				Id = 1,
				Text = "testText",
				UserId = "tabaka10",
				AuthorName = "Tabaka",
				User = new ApplicationUser()
				{
					Id = "tabaka10"
				}
			};

			var editQuote = new AddQuoteViewModel()
			{
				Id=1,
				Text = "editedText",
				AuthorName = "editedName",
			};

			await dbContext.AddAsync(firstQuote);
			await dbContext.SaveChangesAsync();

			await quotesService.Update(editQuote);

			var result = dbContext.Quotes.FirstOrDefault();

			Assert.NotNull(result);
			Assert.AreEqual("editedText", result.Text);
			Assert.AreEqual("editedName", result.AuthorName);
			Assert.AreEqual("tabaka10", result.UserId);
		}

		[Test]
		public async Task GetQuoteForEditShouldGetIdAndUserIdCorrectly()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var quotesService = new QuotesService(dbContext);

			var quote = new Quote()
			{
				Id = 4,
				UserId = "tabaka10",
				Text = "abcdefg",
				User = new ApplicationUser
				{
					Id = "tabaka10",
					UserName = "a10tabakov"
				},

			};
			await dbContext.Quotes.AddAsync(quote);
			await dbContext.SaveChangesAsync();

			var result = quotesService.GetQuoteForEdit(4, "tabaka10");

			Assert.NotNull(result);
		}
	}
}
