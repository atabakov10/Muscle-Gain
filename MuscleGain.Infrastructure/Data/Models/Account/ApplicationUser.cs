
using Microsoft.AspNetCore.Identity;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Reviews;
using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Infrastructure.Data.Models.Account
{
	public class ApplicationUser : IdentityUser
	{
		[Url]
		public string? ImageUrl { get; set; }

		[StringLength(DataConstants.FirstNameMaxLength)]
		public string? FirstName { get; set; }

		[StringLength(DataConstants.LastNameMaxLength)]
		public string? LastName { get; set; }

		public int? ShoppingCartId { get; set; }
		public ShoppingCart? ShoppingCart { get; set; }

		public ICollection<Protein.Protein> ProteinsCreated { get; set; } = new HashSet<Protein.Protein>();
		public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
	}
}
