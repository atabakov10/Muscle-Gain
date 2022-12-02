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

        double GetAverageRating(int courseId);
    }
}
