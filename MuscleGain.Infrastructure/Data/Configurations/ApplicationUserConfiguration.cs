using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;


namespace MuscleGain.Infrastructure.Data.Configurations
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder
				.HasOne(x => x.ShoppingCart)
				.WithOne(x => x.User)
				.HasForeignKey<ShoppingCart>(x => x.UserId)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
