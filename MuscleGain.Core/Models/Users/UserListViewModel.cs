using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Users
{
    public class UserListViewModel
    {
	    public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        [Display(Name = "Phone number")]
		public string? PhoneNumber { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Display(Name = "E-mail")] 
        public string Email { get; set; } = null!;

		[Display(Name = "Image URL")] 
		public string? ImageUrl { get; set; }
	}
}
