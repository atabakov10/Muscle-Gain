using MuscleGain.Infrastructure.Data.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuscleGain.Infrastructure.Data.Models.Protein
{
	public class Order
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string CustomerId { get; set; } = null!;
		public ApplicationUser Customer { get; set; } = null!;

		public DateTime OrderDate { get; set; }

		[Column(TypeName = "decimal(18, 2)")]
		public decimal OrderTotal { get; set; }

		[Required]
		[StringLength(DataConstants.PaymentStatusMaxLength)]
		public string PaymentStatus { get; set; } = null!;

		[Required]
		[StringLength(DataConstants.OrderStatusMaxLength)]
		public string OrderStatus { get; set; } = null!;

		[Required]
		[StringLength(DataConstants.TransactionIdMaxLength)]
		public string TransactionId { get; set; } = null!;

		public ICollection<Protein> Proteins { get; set; } = new HashSet<Protein>();
	}
}
