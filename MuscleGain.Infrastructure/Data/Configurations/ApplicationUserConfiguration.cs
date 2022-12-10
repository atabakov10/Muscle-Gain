using Microsoft.AspNetCore.Identity;
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

			builder.HasData(CreateUsers());

		}

		private List<ApplicationUser> CreateUsers()
		{
			var users = new List<ApplicationUser>();
			var hasher = new PasswordHasher<ApplicationUser>();

			var user = new ApplicationUser()
			{
				Id = "34875379-5ac4-45df-8bd3-7e3bda353ab2",
				FirstName = "Victoria",
				LastName = "Tosheva",
				UserName = "v_tosheva02@abv.bg",
				NormalizedUserName = "v_tosheva02@abv.bg",
				Email = "v_tosheva02@abv.bg",
				NormalizedEmail = "v_tosheva02@abv.bg",
				EmailConfirmed = true
			};

			user.PasswordHash =
				hasher.HashPassword(user, "!Viktoriq123");

			users.Add(user);

			user = new ApplicationUser()
			{
				Id = "e013049a-50da-453a-a95d-1486b1c9f03f",
				ImageUrl = "https://img.a.transfermarkt.technology/portrait/big/28003-1631171950.jpg?lm=1",
				FirstName = "Angel",
				LastName = "Tabakov",
				UserName = "atabakov99@abv.bg",
				NormalizedUserName = "atabakov99@abv.bg",
				Email = "atabakov99@abv.bg",
				NormalizedEmail = "atabakov99@abv.bg",
				EmailConfirmed = true
			};

			user.PasswordHash =
				hasher.HashPassword(user, "!Angata123");

			users.Add(user);

			return users;
		}
	}
}
