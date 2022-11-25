using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;

namespace MuscleGain.Infrastructure.Data.Configurations
{
	public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
	{
		public void Configure(EntityTypeBuilder<ShoppingCart> builder)
		{
			builder
				.HasOne(x => x.User)
				.WithOne(x => x.ShoppingCart)
				.HasForeignKey<ApplicationUser>(x => x.ShoppingCartId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
