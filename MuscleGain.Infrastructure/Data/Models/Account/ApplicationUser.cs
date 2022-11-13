
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Reviews;

namespace MuscleGain.Infrastructure.Data.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(DataConstants.FirstNameMaxLength)]
        public string? FirstName { get; set; }
        [StringLength(DataConstants.LastNameMaxLength)]
        public string? LastName { get; set; }

        public int? ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
	}
}
