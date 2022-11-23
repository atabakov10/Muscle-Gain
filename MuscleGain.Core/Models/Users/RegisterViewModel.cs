using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Users
{
    public class RegisterViewModel
    {
        [Url]
        [Display(Name = "Image URL")]
		public string? ImageUrl { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [Compare(nameof(PasswordRepeat))]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        public string PasswordRepeat { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

    }
}
