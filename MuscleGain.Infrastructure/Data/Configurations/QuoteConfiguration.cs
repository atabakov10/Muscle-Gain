using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleGain.Infrastructure.Data.Models.Protein;
using MuscleGain.Infrastructure.Data.Models.Quotes;

namespace MuscleGain.Infrastructure.Data.Configurations
{
	public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
	{
		public void Configure(EntityTypeBuilder<Quote> builder)
		{
			builder
				.HasOne(u => u.User)
				.WithMany(r => r.Quotes)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
