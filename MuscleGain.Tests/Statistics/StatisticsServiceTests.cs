using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Services.Statistics;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Tests.Statistics
{
    [TestFixture]
    public class StatisticsServiceTests
    {
        [Test]
        public async Task TotalShouldReturnTotalProteinsAndUsers()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var statisticsService = new StatisticsService(dbContext);

            var user = new ApplicationUser()
            {
                Id = "tabaka10"
            };

            var category = new ProteinsCategories()
            {
                Id = 2,
                Name = "Whey category"
            };
            var protein = new Infrastructure.Data.Models.Protein.Protein()
            {
                Id = 1,
                Name = "Whey Protein",
                ApplicationUserId = user.Id,
                CategoryId = category.Id,
                Flavour = "Whey flavour",
                Description = "blablablabla",
                Grams = "500g",
                ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
                IsApproved = true
            };

            await dbContext.Proteins.AddAsync(protein);
            await dbContext.Users.AddAsync(user);
            await dbContext.ProteinsCategories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var allProteins = await dbContext.Proteins.CountAsync();
            var allUsers = await dbContext.Users.CountAsync();


            Assert.That(statisticsService.Total(), Is.Not.Null);
            Assert.That(allUsers,Is.EqualTo(1));
            Assert.That(allProteins, Is.EqualTo(1));
        }
    }
}
