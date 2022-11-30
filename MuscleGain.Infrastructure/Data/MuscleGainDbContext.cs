using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data.Configurations;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Infrastructure.Data.Models.Quotes;
using MuscleGain.Infrastructure.Data.Models.Reviews;

namespace MuscleGain.Infrastructure.Data
{
    public class MuscleGainDbContext : IdentityDbContext<ApplicationUser>
    {
        public MuscleGainDbContext(DbContextOptions<MuscleGainDbContext> options)
            : base(options)
        {
        }
        public DbSet<Protein> Proteins { get; init; }

        public DbSet<ProteinsCategories> ProteinsCategories { get; init; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.ApplyConfiguration(new ApplicationUserConfiguration());

            builder.ApplyConfiguration(new ProteinConfiguration());

			builder.ApplyConfiguration(new ReviewConfiguration());

            builder.ApplyConfiguration(new ShoppingCartConfiguration());

            builder.ApplyConfiguration(new QuoteConfiguration());

			base.OnModelCreating(builder);
        }
    }
}