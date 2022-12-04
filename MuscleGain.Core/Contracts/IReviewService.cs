using MuscleGain.Core.Models.Reviews;

namespace MuscleGain.Core.Contracts
{
    public interface IReviewService
    {
        Task AddReview(AddReviewViewModel model);

        Task<double>? GetAverageRating(int courseId);
    }
}
