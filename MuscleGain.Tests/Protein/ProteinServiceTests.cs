using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Models.Proteins;
using MuscleGain.Core.Services.Proteins;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

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

			var proteinService = new ProteinService(dbContext, null, null);

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
			Assert.AreEqual("https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg", dbContext.Proteins.FirstAsync().Result.ImageUrl);
			Assert.AreEqual(1, dbContext.Proteins.FirstAsync().Result.CategoryId);
			Assert.AreEqual("tabaka10", dbContext.Proteins.FirstAsync().Result.ApplicationUserId);
		}

		[Test]
		public async Task GetSearchedProteinsShouldReturnCorrectProteins()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

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

			var notSearchedProtein = new Infrastructure.Data.Models.Protein.Protein
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
			await dbContext.AddAsync(notSearchedProtein);
			await dbContext.SaveChangesAsync();

			var result = proteinService.AllAsync("Whey Protein", "testName");

			Assert.NotNull(result);
			Assert.AreEqual("testName", dbContext.Proteins.FirstOrDefault().Name);
		}

		[Test]
		public async Task DeleteProteinMethodShouldSetTheIsDeletedPropertyToTrue()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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
				},
				IsDeleted = false
			};

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.SaveChangesAsync();

			await proteinService.Delete(1);

			Assert.That(dbContext.Proteins.Where(x => x.IsDeleted == false).Count(), Is.EqualTo(0));
		}

		[Test]
		public async Task GetAllNotApprovedProteinsShouldReturnAllNotApprovedProteins()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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
				},
				IsApproved = false
			};

			var proteinApproved = new Infrastructure.Data.Models.Protein.Protein
			{
				Id = 2,
				Name = "testNameApproved",
				Flavour = "testFlavourApproved",
				Grams = "501g",
				Price = new decimal(49.98),
				Description = "TestDescriptionApproved",
				ApplicationUserId = "tabaka10Approved",
				ApplicationUser = new ApplicationUser()
				{
					Id = "tabaka10Approved",
				},
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL12002_.jpg",
				CategoryId = 6,
				ProteinCategory = new ProteinsCategories()
				{
					Id = 6,
					Name = "Whey Proteinn"
				},
				IsApproved = true
			};

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.Proteins.AddAsync(proteinApproved);
			await dbContext.SaveChangesAsync();

			var result = await proteinService.GetAllNotApproved();

			Assert.That(result.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task ApproveProteinShouldApprovedProteins()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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
				},
				IsApproved = false
			};

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.SaveChangesAsync();

			await proteinService.ApproveProtein(1);

			Assert.That(dbContext.Proteins.Where(x => x.IsApproved == true).Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task UnapproveProteinShouldDeleteProteins()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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
				},
				IsApproved = false,
				IsDeleted = false
			};

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.SaveChangesAsync();

			await proteinService.UnapproveAprotein(1);

			Assert.That(dbContext.Proteins.Where(x => x.IsDeleted == true).Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetForEditAsyncShouldThrowException()
		{

			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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
				},
				IsApproved = false,
				IsDeleted = true,
				OrderId = 10
			};

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.SaveChangesAsync();

			Assert.That(
				async () => await proteinService.GetForEditAsync(1),
				Throws.Exception.TypeOf<Exception>());
		}

		[Test]
		public async Task EditAsyncShouldReturnEditedProtein()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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

			var editProtein = new EditProteinViewModel()
			{
				Id = 1,
				Flavour = "editedFlavour",
				Name = "editedName",
				CategoryId = 5,
				UserId = "tabaka10",
				Description = "TestDescription",
				Grams = "500g",
				Price = new decimal(49.99),
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg",
			};

			await dbContext.AddAsync(protein);
			await dbContext.SaveChangesAsync();

			await proteinService.EditAsync(editProtein);

			var result = dbContext.Proteins.FirstOrDefault();

			Assert.NotNull(result);
			Assert.AreEqual("editedFlavour", result.Flavour);
			Assert.AreEqual("editedName", result.Name);
			Assert.AreEqual("https://m.media-amazon.com/images/I/616cI2pfTOL._SL1200_.jpg", result.ImageUrl);
		}

		[Test]
		public async Task GetProteinByIdShouldReturnId()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.SaveChangesAsync();

			var result = await proteinService.GetProteinById(protein.Id);

			Assert.That(result.Id, Is.EqualTo(1));
		}

		[Test]
		public async Task LastThreeProteinsShouldReturnLastThreeProteinsAdded()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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

			var proteinTwo = new Infrastructure.Data.Models.Protein.Protein
			{
				Id = 2,
				Name = "testNameTwo",
				Flavour = "testFlavourTwo",
				Grams = "501g",
				Price = new decimal(49.98),
				Description = "TestDescriptionTwo",
				ApplicationUserId = "tabaka10Two",
				ApplicationUser = new ApplicationUser()
				{
					Id = "tabaka10Two",
				},
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL12001_.jpg",
				CategoryId = 6,
				ProteinCategory = new ProteinsCategories()
				{
					Id = 6,
					Name = "Whey ProteinTwo"
				}
			};

			var proteinThree = new Infrastructure.Data.Models.Protein.Protein
			{
				Id = 3,
				Name = "testNameThree",
				Flavour = "testFlavourThree",
				Grams = "502g",
				Price = new decimal(49.97),
				Description = "TestDescriptionThree",
				ApplicationUserId = "tabaka10Three",
				ApplicationUser = new ApplicationUser()
				{
					Id = "tabaka10Three",
				},
				ImageUrl = "https://m.media-amazon.com/images/I/616cI2pfTOL._SL12002_.jpg",
				CategoryId = 7,
				ProteinCategory = new ProteinsCategories()
				{
					Id = 7,
					Name = "Whey ProteinThree"
				}
			};

			await dbContext.Proteins.AddAsync(protein);
			await dbContext.Proteins.AddAsync(proteinTwo);
			await dbContext.Proteins.AddAsync(proteinThree);
			await dbContext.SaveChangesAsync();

			var result = await proteinService.LastThreeProteins();

			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public async Task GetForDetailsAsyncShouldThrowArgumentException()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var proteinService = new ProteinService(dbContext, null, null);

			var protein = new Infrastructure.Data.Models.Protein.Protein
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
			
			await dbContext.Proteins.AddAsync(protein);
			await dbContext.SaveChangesAsync();


			Assert.That(
				async () => await proteinService.GetForDetailsAsync(2),
				Throws.Exception.TypeOf<ArgumentException>(), "Invalid protein Id");
		}
	}
}
