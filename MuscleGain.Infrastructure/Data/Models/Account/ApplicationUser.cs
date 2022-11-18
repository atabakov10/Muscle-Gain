
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Reviews;

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
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
	}
}
