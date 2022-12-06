using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleGain.Infrastructure.Data.Models.Reviews;

namespace MuscleGain.Infrastructure.Data.Configurations
{
	internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder
				.HasOne(u => u.User)
				.WithMany(r => r.Reviews)
				.OnDelete(DeleteBehavior.Restrict);

			builder
				.HasOne(c => c.Protein)
				.WithMany(r => r.Reviews)
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}
