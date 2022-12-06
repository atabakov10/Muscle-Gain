using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Models.Reviews;
using MuscleGain.Core.Services.Review;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;

namespace MuscleGain.Tests.Review
{
	[TestFixture]
	public class ReviewServiceTests
	{

		[Test]
		public async Task CreateMethodShouldAddCorrectNewReviewToDbAndToProtein()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var commentsService = new ReviewService(dbContext);

			var comment = new Infrastructure.Data.Models.Reviews.Review
			{
				Comment = "testContent",
				UserId = "a10tabakov",
				Rating = 5,
				ProteinId = 1
			};

			var commentList = new List<Infrastructure.Data.Models.Reviews.Review>();
			commentList.Add(comment);

			var protein = new Infrastructure.Data.Models.Protein.Protein()
			{
				Id = 1,
				Name = "WheyProtein",
				Grams = "500g",
				Flavour = "Chocolate",
				Price = new decimal(49.99),
				Description = "The best protein ever",
				ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
				OrderId = null,
				IsDeleted = false,
				IsApproved = true,
				CategoryId = 2,
				ApplicationUserId = "a10tabakov",
				Reviews = commentList,
			};

			await dbContext.Proteins.AddAsync(protein);

			var user = new ApplicationUser
			{
				Id = "a10tabakov",
			};

			await dbContext.Users.AddAsync(user);

			var commentToAdd = new AddReviewViewModel()
			{
				ProteinId = 1,
				Rating = 5,
				Comment = "testContent",
				UserId = "a10tabakov",
			};

			await commentsService.AddReview(commentToAdd);

			Assert.NotNull(dbContext.Reviews.FirstOrDefaultAsync());
			Assert.AreEqual("testContent", dbContext.Reviews.FirstAsync().Result.Comment);
			Assert.AreEqual("a10tabakov", dbContext.Reviews.FirstAsync().Result.UserId);
			Assert.AreEqual(5, dbContext.Reviews.FirstAsync().Result.Rating);
			Assert.AreEqual(1, dbContext.Reviews.FirstAsync().Result.ProteinId);
		}

		[Test]
		public async Task GetAverageRatingShouldReturnAverageRatingOnAllReviews()
		{
			var optionsBuilder = new DbContextOptionsBuilder<MuscleGainDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new MuscleGainDbContext(optionsBuilder.Options);

			var reviewService = new ReviewService(dbContext);

			var review = new Infrastructure.Data.Models.Reviews.Review
			{
				Id = 1,
				Comment = "very nice protein!",
				ProteinId = 2,
				Protein = new Infrastructure.Data.Models.Protein.Protein()
				{
					Id = 2,
					ApplicationUserId = "tabaka10",
					Description = "The best protein ever...",
					Name = "Angel Protein",
					Flavour = "Coconut",
					Price = new decimal(44.99),
					ImageUrl = "https://m.media-amazon.com/images/I/41MUAw30QzL._AC_.jpg",
					Grams = "500g"
				},
				Rating = 5,
				UserId = "tabaka10",
				User = new ApplicationUser()
				{
					Id = "tabaka10"
				},
			};
			var reviewTwo = new Infrastructure.Data.Models.Reviews.Review
			{
				Id = 2,
				Comment = "very nice protein!",
				ProteinId = 2,
				Rating = 2,
				UserId = "tabaka10",
			};
			await dbContext.Reviews.AddAsync(review);
			await dbContext.Reviews.AddAsync(reviewTwo);
			await dbContext.SaveChangesAsync();

			var result = await reviewService.GetAverageRating(2);


			Assert.NotNull(result);
			Assert.That(result, Is.EqualTo(3.5));
		}

	}
}
