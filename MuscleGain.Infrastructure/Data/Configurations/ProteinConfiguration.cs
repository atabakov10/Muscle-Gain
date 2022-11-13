using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MuscleGain.Infrastructure.Data.Models.Protein;

namespace MuscleGain.Infrastructure.Data.Configurations
{
	internal class ProteinConfiguration : IEntityTypeConfiguration<Protein>
	{
		public void Configure(EntityTypeBuilder<Protein> builder)
		{
			builder
				.HasOne(c => c.ProteinCategory)
				.WithMany(c => c.Proteins)
				.HasForeignKey(c => c.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
