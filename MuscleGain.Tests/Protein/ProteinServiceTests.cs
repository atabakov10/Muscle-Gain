using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Services.Categories;
using MuscleGain.Core.Services.Proteins;
using MuscleGain.Infrastructure.Data;
using System.Diagnostics;
using MuscleGain.Core.Models.Category;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Tests.Protein
{
	[TestFixture]
	public class ProteinServiceTests
	{
		[Test]
		public async Task AddAsyncMethodShouldAddCorrectNewProteinToDb()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null);

			var proteinToAdd = new AddProtein()
			{
				Name = "testName",
				Flavour = "testFlavour",
				Grams = "500g",
				Price = new decimal(49.99),
				Description = "TestDescription",
				UserId = "tabaka10",
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg",
				CategoryId = 1,

			};

			await proteinService.AddAsync(proteinToAdd);

			Assert.NotNull(dbContext.Proteins.FirstOrDefaultAsync());
			Assert.AreEqual("testName", dbContext.Proteins.FirstAsync().Result.Name);
			Assert.AreEqual("testFlavour", dbContext.Proteins.FirstAsync().Result.Flavour);
			Assert.AreEqual("500g", dbContext.Proteins.FirstAsync().Result.Grams);
			Assert.AreEqual(49.99, dbContext.Proteins.FirstAsync().Result.Price);
			Assert.AreEqual("TestDescription", dbContext.Proteins.FirstAsync().Result.Description);
			Assert.AreEqual("https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg",
				dbContext.Proteins.FirstAsync().Result.ImageUrl);
			Assert.AreEqual(1, dbContext.Proteins.FirstAsync().Result.CategoryId);
			Assert.AreEqual("tabaka10", dbContext.Proteins.FirstAsync().Result.ApplicationUserId);
		}

		[Test]
		public async Task GetSearchedArticlesShouldReturnCorrectArticles()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null);

			var searchedProtein = new Infrastructure.Data.Models.Protein.Protein
			{
				Id = 1,
				Name = "testName",
				Flavour = "testFlavour",
				Grams = "500g",
				Price = new decimal(49.99),
				Description = "TestDescription",
				ApplicationUserId = "tabaka10",
				ApplicationUser = new ApplicationUser()
				{
					Id = "tabaka10",
				},
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg",
				CategoryId = 5,
				ProteinCategory = new ProteinsCategories()
				{
					Id = 5,
					Name = "Whey Protein"
				}
			};

			var notSearchedArticle = new Infrastructure.Data.Models.Protein.Protein
			{
				Id = 2,
				Name = "notTestName",
				Flavour = "notTestFlavour",
				Grams = "not500g",
				Price = new decimal(49.98),
				Description = "notTestDescription",
				ApplicationUserId = "notTabaka10",
				ApplicationUser = new ApplicationUser()
				{
					Id = "notTabaka10",
				},
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg",
				CategoryId = 7,
				ProteinCategory = new ProteinsCategories()
				{
					Id = 7,
					Name = "NotWhey Protein"
				}
			};


			await dbContext.AddAsync(searchedProtein);
			await dbContext.AddAsync(notSearchedArticle);
			await dbContext.SaveChangesAsync();

			var result = proteinService.AllAsync("Whey Protein", "testName");

			Assert.NotNull(result);
			Assert.AreEqual("testName", dbContext.Proteins.FirstOrDefault().Name);
		}
	}
}
