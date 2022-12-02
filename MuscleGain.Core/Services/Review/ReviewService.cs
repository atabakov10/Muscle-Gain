using MuscleGain.Core.Models.Reviews;
using MuscleGain.Infrastructure.Data.Common;
using MuscleGain.Infrastructure.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MuscleGain.Core.Contracts;
using MuscleGain.Infrastructure.Data;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly MuscleGainDbContext dbContext;

        public ReviewService(MuscleGainDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddReview(AddReviewViewModel model)
        {
            var user = await this.dbContext.FindAsync<ApplicationUser>(model.UserId);
            if (user == null)
            {
                throw new Exception();
            }

            var protein = await this.dbContext.FindAsync<Protein>(model.ProteinId);
            if (protein == null)
            {
                throw new Exception();
            }

            var review = new Infrastructure.Data.Models.Reviews.Review()
            {
                Comment = model.Comment,
                Rating = model.Rating,
                ProteinId = model.ProteinId,
                UserId = model.UserId,
                DateOfPublication = model.DateOfPublication,
            };

            await this.dbContext.AddAsync(review);
            await this.dbContext.SaveChangesAsync();
        }

        public double GetAverageRating(int proteinId)
        {
            var averageRating = this.dbContext
                .Reviews
                .Where(c => c.ProteinId == proteinId)
                .Average(r => r.Rating);
            return averageRating;
        }
    }
}
