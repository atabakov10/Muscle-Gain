using Moq;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Services.Categories;
using MuscleGain.Infrastructure.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Test.Categories
{
    public class CategoryServiceTests
    {
        private readonly CategoryService categoryService;
        private readonly Mock<IRepository> categoryMock = new Mock<IRepository>();

        public CategoryServiceTests()
        {
            categoryService = new CategoryService(categoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            //Arrange
            var categoryId = Guid.NewGuid;
            //Act
            var category = await categoryService.CheckForCategory(categoryId);

            //Assert
        }
    }
}
