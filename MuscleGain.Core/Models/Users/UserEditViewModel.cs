using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MuscleGain.Core.Models.Users
{
    public class UserEditViewModel
    {
        [Key]
        public string Id { get; set; }
        public IFormFile? ImageUrl { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
		public string? PhoneNumber { get; set; }

		[Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
    }
}
