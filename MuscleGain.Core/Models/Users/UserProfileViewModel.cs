﻿using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Core.Models.Users
{
    public class UserProfileViewModel
    {
	    public string Id { get; set; } = null!;

		[Display(Name = "Image URL")] 
		public string? ImageUrl { get; set; }

        [Display(Name = "Phone number")] 
        public string? PhoneNumber { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")] 
        public string? LastName { get; set; } = null!;

        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;
    }
}
