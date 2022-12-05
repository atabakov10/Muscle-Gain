using Microsoft.AspNetCore.Mvc;
using MuscleGain.Core.Contracts;
using MuscleGain.Core.Models.Reviews;
using System.Security.Claims;
using Ganss.Xss;

namespace MuscleGain.Controllers
{
    public class ReviewController : BaseController
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult CreateReview(int id)
        {
            var userId = this.GetUserId();

            var model = new AddReviewViewModel()
            {
                UserId = userId,
                ProteinId = id,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(AddReviewViewModel model)
        {
	        var sanitizer = new HtmlSanitizer();
	        model.Comment = sanitizer.Sanitize(model.Comment);
            model.DateOfPublication = DateTime.Now;

            if (!this.ModelState.IsValid)
            {
                return this.View("CreateReview", model);
            }

            await this.reviewService.AddReview(model);
            return this.RedirectToAction("Details", "Protein", new { id = model.ProteinId });
        }

        private string GetUserId()
            => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
