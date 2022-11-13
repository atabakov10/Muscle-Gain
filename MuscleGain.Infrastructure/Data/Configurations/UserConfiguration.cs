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
	internal class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
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
