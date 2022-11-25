using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MuscleGain.Infrastructure.Data.DataConstants;
using Microsoft.AspNetCore.Identity;
using MuscleGain.Infrastructure.Data.Models.Account;
using MuscleGain.Infrastructure.Data.Models.Reviews;
using MuscleGain.Infrastructure.Data.Models.Cart;

namespace MuscleGain.Infrastructure.Data.Models.Protein
{
    public class Protein
    {
        [Key]
        public int Id { get; init; }
        
        [Required]
        [StringLength(ProteinNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(ProteinGramsMaxLength)]
        public string Grams { get; set; }

        [Required]
        [StringLength(ProteinFlavorMaxLength)]
        public string Flavour { get; set; }

        [Required]
        [MaxLength(ProteinPriceMaxLength)]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public ProteinsCategories ProteinCategory { get; init; }

        [NotMapped]
        public double Rating
            => this.Reviews.Count > 0 ? this.Reviews.Average(x => x.Rating) : 0;
        public ICollection<ShoppingCart> ShoppingCart { get; set; } = new HashSet<ShoppingCart>();

        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

	}
}
