using MuscleGain.Core.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuscleGain.Core.Contracts
{
    public interface IReviewService
    {
        Task AddReview(AddReviewViewModel model);

        Task DeleteReview(int id, string userId);

        Task<EditReviewViewModel> GetReviewForEdit(int id, string userId);

        Task Update(EditReviewViewModel model, string userId);

        double GetAverageRating(int courseId);
    }
}
