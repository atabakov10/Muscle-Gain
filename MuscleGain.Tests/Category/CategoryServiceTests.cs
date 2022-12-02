using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Category;
using MuscleGain.Core.Services.Categories;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Tests.Category
{
    [TestFixture]
    public class CategoryServiceTests
    {

        [Test]
        public async Task CreateMethodShouldAddCorrectNewCategoryToDb()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var categoryService = new CategoryService(dbContext);

            var categoryToAdd = new ProteinCategoryViewModel()
            {
                Name = "testName",
            };

            await categoryService.CreateCategory(categoryToAdd);

            Assert.NotNull(dbContext.ProteinsCategories.FirstOrDefaultAsync());
            Assert.That(dbContext.ProteinsCategories.FirstAsync().Result.Name, Is.EqualTo("testName"));
        }

        [Test]
        public async Task DeleteMethodShouldDeleteCategoriesById()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var categoryService = new CategoryService(dbContext);

            var category = new ProteinsCategories()
            {
                Id = 5,
                Name = "testName",
                IsDeleted = false
            };
            await dbContext.ProteinsCategories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            await categoryService.Delete(5);

            Assert.False(dbContext.ProteinsCategories.Any(x=> x.IsDeleted == false));
        }

        [Test]

        public async Task GetAllShouldReturnAllCategories()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

            var categoryService = new CategoryService(dbContext);

            var firstCategory = new ProteinsCategories()
            {
                Id = 5,
                Name = "testName",
                IsDeleted = false
            };

            var secondCategory = new ProteinsCategories()
            {
                Id = 4,
                Name = "testName",
                IsDeleted = false
            };

            await dbContext.ProteinsCategories.AddAsync(firstCategory);
            await dbContext.ProteinsCategories.AddAsync(secondCategory);
            await dbContext.SaveChangesAsync();

            var result = await categoryService.GetAllCategories();

            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.AreEqual("testName", result.FirstOrDefault().Name);
            Assert.AreEqual(5, result.FirstOrDefault().Id);
        }
    }
}
