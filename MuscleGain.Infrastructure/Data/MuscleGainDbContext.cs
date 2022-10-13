using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuscleGain.Infrastructure.Data.Models;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Protein;

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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Protein>()
                .HasOne(c => c.ProteinCategory)
                .WithMany(c => c.Proteins)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}