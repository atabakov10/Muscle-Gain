using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Users
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Url]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
		public string? PhoneNumber { get; set; }

		[Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
