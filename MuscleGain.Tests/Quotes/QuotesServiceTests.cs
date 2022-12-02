using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Services.Quote;
using MuscleGain.Infrastructure.Data.Models.Quotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleGain.Core.Models.Quotes;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;

namespace MuscleGain.Tests.Quotes
{
    [TestFixture]
    public class QuotesServiceTests
    {
        [Test]
        public async Task GetAllShouldReturnAllCategories()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var quotesService = new QuotesService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "a10tabakov"
            };
            var firstQuote = new AddQuoteViewModel()
            {
                Id = 1,
                Text = "testContent",
                UserId = user.Id,
            };

            var secondQuote = new AddQuoteViewModel()
            {
                Id = 2,
                Text = "testContent",
                UserId = user.Id,
            };



            await quotesService.Add(firstQuote);
            await quotesService.Add(secondQuote);

            var result = await quotesService.GetAll();

            Assert.NotNull(result.Count());
            Assert.AreEqual("testContent", dbContext.Quotes.FindAsync().Result.Text);
            Assert.AreEqual("a10tabakov", dbContext.Quotes.FindAsync().Result.UserId);

        }
    }
}
