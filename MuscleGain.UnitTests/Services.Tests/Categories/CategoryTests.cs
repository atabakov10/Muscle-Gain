using Moq;
using MuscleGain.Core.Contracts;

namespace MuscleGain.UnitTests.Services.Tests.Categories
{
	[TestFixture]
	public class CategoryTests
	{
	

		[Test]
		public void ChecksCategory_ShouldReturnCorrectResult()
		{
			var mockCategoryService = new Mock<ICategoryService>();
			//arrange
			mockCategoryService.Setup(x => x.CheckForCategory(1).Id).Returns(1);

			
		}
	}
}
