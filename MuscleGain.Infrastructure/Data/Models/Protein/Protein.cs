﻿using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Cart;
using MuscleGain.Infrastructure.Data.Models.Reviews;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MuscleGain.Infrastructure.Data.DataConstants;

namespace MuscleGain.Infrastructure.Data.Models.Protein
{
    public class Protein
    {
        [Key]
        public int Id { get; init; }
        
        [Required]
        [StringLength(ProteinNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(ProteinGramsMaxLength)]
        public string Grams { get; set; } = null!;

        [Required]
        [StringLength(ProteinFlavorMaxLength)]
        public string Flavour { get; set; } = null!;

        [Required]
        [MaxLength(ProteinPriceMaxLength)]
        public decimal Price { get; set; }

        [Required] public string Description { get; set; } = null!;

        [Required]
        [Url]
        public string ImageUrl { get; set; } = null!;

        public bool IsDeleted { get; set; }
        public bool IsApproved { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int CategoryId { get; set; }

		public ProteinsCategories ProteinCategory { get; init; } = null!;

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [NotMapped]
        public double Rating
            => this.Reviews.Count > 0 ? this.Reviews.Average(x => x.Rating) : 0;
        public ICollection<ShoppingCart> ShoppingCart { get; set; } = new HashSet<ShoppingCart>();

        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

	}
}
