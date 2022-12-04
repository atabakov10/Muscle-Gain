using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Category;
using MuscleGain.Core.Models.Reviews;
using MuscleGain.Core.Services.Review;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Tests.Review
{
    [TestFixture]
    public class ReviewServiceTests
    {

        [Test]
        public async Task CreateMethodShouldAddCorrectNewCommentToDbAndToArticle()
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
    }
}
