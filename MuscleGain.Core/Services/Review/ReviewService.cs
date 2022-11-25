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
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Core.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository repository;

        public ReviewService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddReview(AddReviewViewModel model)
        {
            var user = await this.repository.GetByIdAsync<ApplicationUser>(model.UserId);
            if (user == null)
            {
                throw new Exception();
            }

            var course = await this.repository.GetByIdAsync<Protein>(model.ProteinId);
            if (course == null)
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

            await this.repository.AddAsync(review);
            await this.repository.SaveChangesAsync();
        }

        public async Task DeleteReview(int id, string userId)
        {
            var user = await this.repository.GetByIdAsync<ApplicationUser>(userId);
            if (user == null)
            {
                throw new Exception();
            }

            var review = await this.repository.GetByIdAsync<Infrastructure.Data.Models.Reviews.Review>(id);
            if (review == null)
            {
                throw new Exception();
            }

            if (review.UserId != user.Id)
            {
                throw new Exception("Invalid operation");
            }

            await repository.DeleteAsync<Infrastructure.Data.Models.Reviews.Review>(review);
            await this.repository.SaveChangesAsync();
        }

        public async Task Update(EditReviewViewModel model, string userId)
        {
            var review = await this.repository.GetByIdAsync<Infrastructure.Data.Models.Reviews.Review>(model.Id);
            if (review == null)
            {
                throw new Exception();
            }

            var user = await this.repository.GetByIdAsync<ApplicationUser>(userId);
            if (user == null)
            {
                throw new Exception();
            }

            if (model.UserId != user.Id)
            {
                throw new Exception();
            }

            review.Rating = model.Rating;
            review.Comment = model.Comment;
            review.LastUpdate = model.LastUpdate;

            await this.repository.SaveChangesAsync();
        }

        public async Task<EditReviewViewModel> GetReviewForEdit(int id, string userId)
        {
            var model = await this.repository.GetByIdAsync<Infrastructure.Data.Models.Reviews.Review>(id);
            if (model == null)
            {
                throw new Exception();
            }

            var user = await this.repository.GetByIdAsync<ApplicationUser>(userId);
            if (user == null)
            {
                throw new Exception();
            }

            if (model.UserId != user.Id)
            {
                throw new Exception();
            }

            return new EditReviewViewModel()
            {
                Id = model.Id,
                Rating = (int)model.Rating,
                Comment = model.Comment,
                UserId = model.UserId,
            };
        }

        public double GetAverageRating(int proteinId)
        {
            var averageRating = this.repository
                .AllReadonly<Infrastructure.Data.Models.Reviews.Review>()
                .Where(c => c.ProteinId == proteinId)
                .Average(r => r.Rating);
            return averageRating;
        }
    }
}
