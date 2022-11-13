using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;

namespace MuscleGain.Infrastructure.Data.Configurations
{
	internal class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
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
