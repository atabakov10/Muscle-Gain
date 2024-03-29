﻿using MuscleGain.Infrastructure.Data.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace MuscleGain.Infrastructure.Data.Models.Reviews
{
	public class Review
	{
		public int Id { get; set; }

		[Required]
		[StringLength(DataConstants.CommentMaxLength)]
		public string Comment { get; set; } = null!;

		[MaxLength(DataConstants.RatingMaxLength)]
		public double Rating { get; set; }

        public DateTime DateOfPublication { get; set; }

        public string UserId { get; set; } = null!;
		public ApplicationUser User { get; set; } = null!;

		public int ProteinId { get; set; }
		public Protein.Protein Protein { get; set; } = null!;
	}
}
